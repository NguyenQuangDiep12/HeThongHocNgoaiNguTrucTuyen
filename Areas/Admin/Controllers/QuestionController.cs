using HeThongHocNgoaiNguTrucTuyen.Dtos.Requests;
using HeThongHocNgoaiNguTrucTuyen.Dtos.Responses;
using HeThongHocNgoaiNguTrucTuyen.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HeThongHocNgoaiNguTrucTuyen.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "ADMIN")]
    public class QuestionController : Controller
    {
        private readonly IQuestionService _questionService;

        private readonly ITestService _testService;

        public QuestionController(
            IQuestionService questionService,
            ITestService testService)
        {
            _questionService = questionService;
            _testService = testService;
        }

        // =====================================================
        // INDEX
        // =====================================================

        [HttpGet]
        public async Task<IActionResult> Index(
            QuestionRequest request,
            int pageSize = 10,
            int pageNumber = 1,
            CancellationToken ct = default)
        {
            pageNumber =
                pageNumber <= 0
                    ? 1
                    : pageNumber;

            pageSize =
                pageSize <= 0
                    ? 10
                    : Math.Min(pageSize, 10);

            var questions =
                await _questionService.GetQuestionsAsync(
                    pageSize,
                    pageNumber,
                    request,
                    ct);

            var questionCount =
                await _questionService.CountQuestionsAsync(
                    request,
                    ct);

            // Lấy toàn bộ Test để filter
            ViewBag.Tests =
                await _testService.GetTestsAsync(
                    100,
                    1,
                    null,
                    ct);

            ViewBag.PageNumber = pageNumber;

            ViewBag.PageSize = pageSize;

            ViewBag.TotalPages =
                Math.Max(
                    1,
                    (int)Math.Ceiling(
                        (decimal)questionCount / pageSize));

            ViewBag.FilterTestId = request.TestId;

            ViewBag.FilterPartNumber =
                request.PartNumber;

            return View(questions);
        }

        // =====================================================
        // CREATE GET
        // =====================================================

        [HttpGet]
        public async Task<IActionResult> Create(
            CancellationToken ct)
        {
            ViewBag.Tests =
                await _testService.GetTestsAsync(
                    100,
                    1,
                    null,
                    ct);

            return View(new QuestionRequest());
        }

        // =====================================================
        // CREATE POST
        // =====================================================

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(
            QuestionRequest request,
            CancellationToken ct)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Tests =
                    await _testService.GetTestsAsync(
                        100,
                        1,
                        null,
                        ct);

                return View(request);
            }

            try
            {
                await _questionService.CreateQuestionAsync(
                    request,
                    ct);

                TempData["Success"] =
                    "Thêm câu hỏi thành công.";

                return RedirectToAction(nameof(Index));
            }
            catch (ArgumentException ex)
            {
                ModelState.AddModelError(
                    string.Empty,
                    ex.Message);

                ViewBag.Tests =
                    await _testService.GetTestsAsync(
                        100,
                        1,
                        null,
                        ct);

                return View(request);
            }
        }

        // =====================================================
        // EDIT GET
        // =====================================================

        [HttpGet]
        public async Task<IActionResult> Edit(
            int id,
            CancellationToken ct)
        {
            var question =
                await _questionService.GetQuestionByIdAsync(
                    id,
                    ct);

            if (question == null)
            {
                TempData["NotFound"] =
                    "Không tìm thấy câu hỏi.";

                return RedirectToAction(nameof(Index));
            }

            ViewBag.Tests =
                await _testService.GetTestsAsync(
                    100,
                    1,
                    null,
                    ct);

            ViewBag.QuestionId = id;

            return View(new QuestionRequest
            {
                TestId = question.TestId,

                Content = question.Content,

                ImageUrl = question.ImageUrl,

                PartNumber = question.PartNumber,

                QuestionOrder =
                    question.QuestionOrder,

                AudioUrl = question.AudioUrl,

                GroupCode = question.GroupCode
            });
        }

        // =====================================================
        // EDIT POST
        // =====================================================

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(
            int id,
            QuestionRequest request,
            CancellationToken ct)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Tests =
                    await _testService.GetTestsAsync(
                        100,
                        1,
                        null,
                        ct);

                ViewBag.QuestionId = id;

                return View(request);
            }

            try
            {
                var result =
                    await _questionService.UpdateQuestionAsync(
                        id,
                        request,
                        ct);

                if (!result)
                {
                    TempData["NotFound"] =
                        "Không tìm thấy câu hỏi.";
                }
                else
                {
                    TempData["Success"] =
                        "Cập nhật câu hỏi thành công.";
                }

                return RedirectToAction(nameof(Index));
            }
            catch (ArgumentException ex)
            {
                ModelState.AddModelError(
                    string.Empty,
                    ex.Message);

                ViewBag.Tests =
                    await _testService.GetTestsAsync(
                        100,
                        1,
                        null,
                        ct);

                ViewBag.QuestionId = id;

                return View(request);
            }
        }

        // =====================================================
        // DELETE
        // =====================================================

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(
            int id,
            CancellationToken ct)
        {
            var result =
                await _questionService.DeleteQuestionAsync(
                    id,
                    ct);

            if (!result)
            {
                TempData["NotFound"] =
                    "Không tìm thấy câu hỏi cần xóa.";
            }
            else
            {
                TempData["Success"] =
                    "Xóa câu hỏi thành công.";
            }

            return RedirectToAction(nameof(Index));
        }
    }
}