using HeThongHocNgoaiNguTrucTuyen.Dtos.Requests;
using HeThongHocNgoaiNguTrucTuyen.Models.Enums;
using HeThongHocNgoaiNguTrucTuyen.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HeThongHocNgoaiNguTrucTuyen.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "ADMIN")]
    public class TestController : Controller
    {
        private readonly ITestService _testService;

        public TestController(
            ITestService testService)
        {
            _testService = testService;
        }

        // =====================================================
        // INDEX
        // =====================================================

        [HttpGet]
        public async Task<IActionResult> Index(
            [FromQuery] TestRequest request,
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

            var tests =
                await _testService.GetTestsAsync(
                    pageSize,
                    pageNumber,
                    request,
                    ct);

            var totalCount =
                await _testService.CountTestsAsync(
                    request,
                    ct);

            ViewBag.PageNumber = pageNumber;

            ViewBag.PageSize = pageSize;

            ViewBag.TotalPages =
                Math.Max(
                    1,
                    (int)Math.Ceiling(
                        (decimal)totalCount / pageSize));

            return View(tests);
        }

        // =====================================================
        // CREATE GET
        // =====================================================

        [HttpGet]
        public IActionResult Create()
        {
            return View(new TestRequest
            {
                TestMode = TestMode.PART
            });
        }

        // =====================================================
        // CREATE POST
        // =====================================================

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(
            TestRequest request,
            CancellationToken ct)
        {
            if (!ModelState.IsValid)
            {
                return View(request);
            }

            try
            {
                await _testService.CreateTestAsync(
                    request,
                    ct);

                TempData["Success"] =
                    "Tạo bài kiểm tra thành công.";

                return RedirectToAction(nameof(Index));
            }
            catch (ArgumentException ex)
            {
                ModelState.AddModelError(
                    string.Empty,
                    ex.Message);

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
            var test =
                await _testService.GetTestByIdAsync(
                    id,
                    ct);

            if (test == null)
            {
                TempData["NotFound"] =
                    "Không tìm thấy bài kiểm tra.";

                return RedirectToAction(nameof(Index));
            }

            ViewBag.TestId = id;

            return View(new TestRequest
            {
                Title = test.Title,

                Description = test.Description,

                TestMode =
                    (TestMode)test.TestMode,

                PartNumber =
                    test.PartNumber,

                DurationMinutes =
                    test.DurationMinutes
            });
        }

        // =====================================================
        // EDIT POST
        // =====================================================

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(
            int id,
            TestRequest request,
            CancellationToken ct)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.TestId = id;

                return View(request);
            }

            try
            {
                var result =
                    await _testService.UpdateTestAsync(
                        id,
                        request,
                        ct);

                if (!result)
                {
                    TempData["NotFound"] =
                        "Không tìm thấy bài kiểm tra.";
                }
                else
                {
                    TempData["Success"] =
                        "Cập nhật bài kiểm tra thành công.";
                }

                return RedirectToAction(nameof(Index));
            }
            catch (ArgumentException ex)
            {
                ModelState.AddModelError(
                    string.Empty,
                    ex.Message);

                ViewBag.TestId = id;

                return View(request);
            }
            catch (InvalidOperationException ex)
            {
                ModelState.AddModelError(
                    string.Empty,
                    ex.Message);

                ViewBag.TestId = id;

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
            try
            {
                var result =
                    await _testService.DeleteTestAsync(
                        id,
                        ct);

                if (!result)
                {
                    TempData["NotFound"] =
                        "Không tìm thấy bài kiểm tra cần xóa.";
                }
                else
                {
                    TempData["Success"] =
                        "Xóa bài kiểm tra thành công.";
                }
            }
            catch (InvalidOperationException ex)
            {
                TempData["Error"] = ex.Message;
            }

            return RedirectToAction(nameof(Index));
        }
    }
}