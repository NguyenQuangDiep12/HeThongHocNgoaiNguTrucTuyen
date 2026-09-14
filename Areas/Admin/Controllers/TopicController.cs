using HeThongHocNgoaiNguTrucTuyen.Dtos.Requests.Topic;
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

        public TopicController(ITopicService topicService, ILanguageService languageService)
        {
            _topicService = topicService;
            _languageService = languageService;
        }

        [HttpGet]
        public async Task<IActionResult> Index(int languageId, TopicFilterRequest? request, int pageSize = 10, int pageNumber = 1, CancellationToken ct = default)
        {
            if (languageId <= 0)
            {
                return RedirectToAction("Index", "Language");
            }

            request ??= new TopicFilterRequest();

            var language = await _languageService.GetLanguageByIdAsync(languageId, ct);

            if (language == null)
            {
                TempData["NotFound"] = "Không tìm thấy ngôn ngữ.";
                return RedirectToAction("Index", "Language");
            }

            var topics = await _topicService.GetTopicsAsync(languageId, request, pageNumber, pageSize, ct);

            var topicCount = await _topicService.CountTopicsAsync(languageId, request, ct);

            var totalPages = Math.Max(1, (int)Math.Ceiling((double)topicCount / pageSize));

            if (pageNumber > totalPages)
            {
                pageNumber = totalPages;

                topics = await _topicService.GetTopicsAsync(languageId, request, pageNumber, pageSize, ct);
            }

            ViewBag.LanguageId = languageId;
            ViewBag.LanguageName = language.Name;
            ViewBag.Levels = await _topicService.GetLevelsByLanguageIdAsync(languageId, ct);
            ViewBag.PageNumber = pageNumber;
            ViewBag.PageSize = pageSize;
            ViewBag.TotalPages = totalPages;
            ViewBag.Filter = request;

            return View(topics);
        }

        [HttpGet]
        public async Task<IActionResult> Create(int languageId, CancellationToken ct = default)
        {
            if (languageId <= 0)
            {
                return RedirectToAction("Index", "Language");
            }

            var language = await _languageService.GetLanguageByIdAsync(languageId, ct);

            if (language == null)
            {
                TempData["NotFound"] = "Không tìm thấy ngôn ngữ.";
                return RedirectToAction("Index", "Language");
            }

            ViewBag.LanguageId = languageId;
            ViewBag.LanguageName = language.Name;

            return View(new CreateTopicRequest
            {
                LanguageId = languageId
            });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(int languageId, CreateTopicRequest request, CancellationToken ct = default)
        {
            if (languageId <= 0)
            {
                return RedirectToAction("Index", "Language");
            }

            var language = await _languageService.GetLanguageByIdAsync(languageId, ct);

            if (language == null)
            {
                TempData["NotFound"] = "Không tìm thấy ngôn ngữ.";
                return RedirectToAction("Index", "Language");
            }

            if (!ModelState.IsValid)
            {
                ViewBag.LanguageId = languageId;
                ViewBag.LanguageName = language.Name;
                return View(request);
            }

            request.LanguageId = languageId;

            await _topicService.CreateTopicAsync(request, ct);

            return RedirectToAction(nameof(Index), new { languageId });
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id, int languageId, CancellationToken ct = default)
        {
            if (languageId <= 0)
            {
                return RedirectToAction("Index", "Language");
            }

            var topic = await _topicService.GetTopicByIdAsync(id, ct);

            if (topic == null || topic.LanguageId != languageId)
            {
                TempData["NotFound"] = "Chủ đề không thuộc ngôn ngữ này.";
                return RedirectToAction(nameof(Index), new { languageId });
            }

            ViewBag.LanguageId = languageId;
            ViewBag.LanguageName = topic.LanguageName;
            ViewBag.TopicId = id;

            var request = new UpdateTopicRequest
            {
                Name = topic.Name,
                Level = topic.Level ?? string.Empty,
                Description = topic.Description,
                ImageUrl = topic.ImageUrl,
                LanguageId = languageId
            };

            return View(request);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, int languageId, UpdateTopicRequest request, CancellationToken ct = default)
        {
            if (languageId <= 0)
            {
                return RedirectToAction("Index", "Language");
            }

            var topic = await _topicService.GetTopicByIdAsync(id, ct);

            if (topic == null || topic.LanguageId != languageId)
            {
                TempData["NotFound"] = "Chủ đề không thuộc ngôn ngữ này.";
                return RedirectToAction(nameof(Index), new { languageId });
            }

            if (!ModelState.IsValid)
            {
                ViewBag.LanguageId = languageId;
                ViewBag.LanguageName = topic.LanguageName;
                ViewBag.TopicId = id;
                return View(request);
            }

            request.LanguageId = languageId;

            var result = await _topicService.UpdateTopicAsync(id, request, ct);

            if (!result)
            {
                TempData["NotFound"] = "Không tìm thấy chủ đề.";
            }

            return RedirectToAction(nameof(Index), new { languageId });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id, int languageId, CancellationToken ct = default)
        {
            if (languageId <= 0)
            {
                return RedirectToAction("Index", "Language");
            }

            var topic = await _topicService.GetTopicByIdAsync(id, ct);

            if (topic == null || topic.LanguageId != languageId)
            {
                TempData["NotFound"] = "Chủ đề không thuộc ngôn ngữ này.";
                return RedirectToAction(nameof(Index), new { languageId });
            }

            var result = await _topicService.DeleteTopicAsync(id, ct);

            if (!result)
            {
                TempData["NotFound"] = "Không tìm thấy chủ đề.";
            }

            return RedirectToAction(nameof(Index), new { languageId });
        }
    }
}