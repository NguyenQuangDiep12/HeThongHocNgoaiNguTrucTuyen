using HeThongHocNgoaiNguTrucTuyen.Dtos.Requests;
using HeThongHocNgoaiNguTrucTuyen.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HeThongHocNgoaiNguTrucTuyen.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "ADMIN")]
    public class TopicController : Controller
    {
        private readonly ITopicService _topicService;
        private readonly ILanguageService _languageService;

        public TopicController(
            ITopicService topicService,
            ILanguageService languageService)
        {
            _topicService = topicService;
            _languageService = languageService;
        }

        [HttpGet]
        public async Task<IActionResult> Index(
            TopicRequest request,
            int pageSize = 10,
            int pageNumber = 1,
            CancellationToken ct = default)
        {
            pageNumber = pageNumber <= 0 ? 1 : pageNumber;
            pageSize = pageSize <= 0 ? 10 : Math.Min(pageSize, 10);

            var topics = await _topicService.GetTopicsAsync(
                pageSize,
                pageNumber,
                request,
                ct);

            var topicCount = await _topicService.CountTopicsAsync(
                request,
                ct);

            ViewBag.Languages = await _languageService.GetLanguagesAsync(
                100,
                1,
                string.Empty,
                ct);

            ViewBag.Levels = await _topicService.GetLevelsAsync(ct);

            ViewBag.PageNumber = pageNumber;
            ViewBag.PageSize = pageSize;

            ViewBag.TotalPages = Math.Max(
                1,
                (int)Math.Ceiling((decimal)topicCount / pageSize));

            return View(topics);
        }


        // =========================
        // CREATE
        // =========================

        [HttpGet]
        public async Task<IActionResult> Create(
            CancellationToken ct)
        {
            ViewBag.Languages =
                await _languageService.GetLanguagesAsync(
                    100,
                    1,
                    string.Empty,
                    ct);

            return View(new TopicRequest());
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(
            TopicRequest request,
            CancellationToken ct)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Languages =
                    await _languageService.GetLanguagesAsync(
                        100,
                        1,
                        string.Empty,
                        ct);

                return View(request);
            }

            await _topicService.CreateTopicAsync(
                request,
                ct);

            return RedirectToAction(nameof(Index));
        }


        // =========================
        // EDIT
        // =========================

        [HttpGet]
        public async Task<IActionResult> Edit(
            int id,
            CancellationToken ct)
        {
            var topic =
                await _topicService.GetTopicByIdAsync(
                    id,
                    ct);

            if (topic == null)
            {
                TempData["NotFound"] =
                    "Không tìm thấy chủ đề.";

                return RedirectToAction(nameof(Index));
            }

            ViewBag.Languages =
                await _languageService.GetLanguagesAsync(
                    100,
                    1,
                    string.Empty,
                    ct);

            ViewBag.TopicId = id;

            var request = new TopicRequest
            {
                Name = topic.Name,
                Level = topic.Level ?? string.Empty,
                Description = topic.Description,
                ImageUrl = topic.ImageUrl,
                LanguageId = topic.LanguageId,
                LanguageName = topic.LanguageName
            };

            return View(request);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(
            int id,
            TopicRequest request,
            CancellationToken ct)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Languages =
                    await _languageService.GetLanguagesAsync(
                        100,
                        1,
                        string.Empty,
                        ct);

                ViewBag.TopicId = id;

                return View(request);
            }

            var result =
                await _topicService.UpdateTopicAsync(
                    id,
                    request,
                    ct);

            if (!result)
            {
                TempData["NotFound"] =
                    "Không tìm thấy chủ đề.";

                return RedirectToAction(nameof(Index));
            }

            return RedirectToAction(nameof(Index));
        }


        // =========================
        // DELETE
        // =========================

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(
            int id,
            CancellationToken ct)
        {
            var result =
                await _topicService.DeleteTopicAsync(
                    id,
                    ct);

            if (!result)
            {
                TempData["NotFound"] =
                    "Không tìm thấy chủ đề cần xóa.";
            }

            return RedirectToAction(nameof(Index));
        }
    }
}