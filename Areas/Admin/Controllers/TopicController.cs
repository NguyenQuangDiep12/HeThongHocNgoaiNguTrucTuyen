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

        public TopicController(ITopicService topicService, ILanguageService languageService)
        {
            _topicService = topicService;
            _languageService = languageService;
        }

        [HttpGet]
        public async Task<IActionResult> Index(int languageId ,TopicFilterRequest request, int pageSize = 10, int pageNumber = 1, CancellationToken ct = default)
        {
            var topics = await _topicService.GetTopicsAsync(languageId, request, pageSize, pageNumber, ct);
            var topicCount = await _topicService.CountTopicsAsync(languageId, request, ct);
            ViewBag.Languages = await _languageService.GetAllLanguagesAsync(ct);

            if(languageId > 0)
            {
                ViewBag.Levels = await _topicService.GetLevelsByLanguageIdAsync(languageId, ct);
            }
            else
            {
                ViewBag.Levels = new List<string>();
            }

            ViewBag.SelectedLanguageId = languageId;
            ViewBag.SelectedLevel = request?.Level ?? "";

            ViewBag.PageNumber = pageNumber;
            ViewBag.PageSize = pageSize;
            ViewBag.TotalPages = Math.Max(1, (int)Math.Ceiling((decimal)topicCount / pageSize));
            return View(topics);
        }
        [HttpGet]
        public async Task<IActionResult> GetTopicsByLanguageDropdown(int languageId, CancellationToken ct = default)
        {
            var topics = await _topicService.GetTopicsByLanguageIdAsync(languageId, ct);

            // chuyen doi List<TopicInfoResponse> sang List<(name, topicId)>
            var result = topics.Select(t => new
            {
                TopicId = t.TopicId,
                Name = t.Name
            });

            // Gui du lieu dang json cho topic thuoc cung mot language
            return Json(result);
        }

        [HttpGet]
        public async Task<IActionResult> GetLevelsByLanguageId(int languageId, CancellationToken ct)
        {
            if(languageId <= 0)
            {
                return Json(new List<string>());
            }

            var levels = await _topicService.GetLevelsByLanguageIdAsync(languageId, ct);

            return Json(levels);
        }
        [HttpGet]
        public async Task<IActionResult> Detail(int id, CancellationToken ct)
        {
            var topicDetail = await _topicService.GetTopicByIdAsync(id, ct);

            if(topicDetail == null)
            {
                TempData["NotFound"] = "Không tìm thấy bài học";
                return RedirectToAction(nameof(Index));
            }

            return View(topicDetail);
        }
        [HttpGet]
        public async Task<IActionResult> Create(CancellationToken ct)
        {
            var languages = await _languageService.GetAllLanguagesAsync(ct);
            ViewBag.Languages = languages;
            return View(new CreateTopicRequest());
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateTopicRequest request, CancellationToken ct)
        {
            if (!ModelState.IsValid)
            {
                return View(request);
            }
            await _topicService.CreateTopicAsync(request, ct);
            return RedirectToAction(nameof(Index));
        }
        [HttpGet]
        public async Task<IActionResult> Edit( int id, CancellationToken ct)
        {
            var languages = await _languageService.GetAllLanguagesAsync(ct);
            var topic = await _topicService.GetTopicByIdAsync(id, ct);
            if (topic == null)
            {
                TempData["NotFound"] = "Không tìm thấy chủ đề.";
                return RedirectToAction(nameof(Index));
            }
            ViewBag.TopicId = id;
            ViewBag.Languages = languages;
            var request = new UpdateTopicRequest
            {
                Name = topic.Name,
                Level = topic.Level ?? string.Empty,
                Description = topic.Description,
                ImageUrl = topic.ImageUrl,
                LanguageId = topic.LanguageId,
            };
            return View(request);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, UpdateTopicRequest request, CancellationToken ct)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.TopicId = id;
                return View(request);
            }
            var result = await _topicService.UpdateTopicAsync(id, request, ct);
            if (!result)
            {
                TempData["NotFound"] = "Không tìm thấy chủ đề.";
                return RedirectToAction(nameof(Index));
            }
            return RedirectToAction(nameof(Index));
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id, CancellationToken ct)
        {
            var result = await _topicService.DeleteTopicAsync( id, ct);
            if (!result)
            {
                TempData["NotFound"] = "Không tìm thấy chủ đề cần xóa.";
            }
            return RedirectToAction(nameof(Index));
        }
    }
}