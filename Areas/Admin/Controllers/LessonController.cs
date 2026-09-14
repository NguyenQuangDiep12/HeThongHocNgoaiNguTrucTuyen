using HeThongHocNgoaiNguTrucTuyen.Dtos.Requests.Lesson;
using HeThongHocNgoaiNguTrucTuyen.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HeThongHocNgoaiNguTrucTuyen.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "ADMIN")]
    public class LessonController : Controller
    {
        private readonly ILessonService _lessonService;
        private readonly ITopicService _topicService;

        public LessonController(ILessonService lessonService, ITopicService topicService)
        {
            _lessonService = lessonService;
            _topicService = topicService;
        }

        [HttpGet]
        public async Task<IActionResult> Index(int topicId, LessonFilterRequest? request = null, int pageNumber = 1, int pageSize = 10, CancellationToken ct = default)
        {
            if (topicId <= 0)
            {
                return RedirectToAction("Index", "Language");
            }

            request ??= new LessonFilterRequest();

            var topic = await _topicService.GetTopicByIdAsync(topicId, ct);
            if (topic == null)
            {
                TempData["NotFound"] = "Không tìm thấy chủ đề.";
                return RedirectToAction("Index", "Language");
            }

            var totalItems = await _lessonService.CountLessonsAsync(topicId, request, ct);
            var totalPages = Math.Max(1, (int)Math.Ceiling(totalItems / (double)pageSize));
            if (pageNumber > totalPages)
            {
                pageNumber = totalPages;
            }

            var lessons = await _lessonService.GetLessonsAsync(topicId, request, pageNumber, pageSize, ct);

            ViewBag.TopicId = topicId;
            ViewBag.TopicName = topic.Name;
            ViewBag.LanguageId = topic.LanguageId;
            ViewBag.LanguageName = topic.LanguageName;
            ViewBag.PageNumber = pageNumber;
            ViewBag.PageSize = pageSize;
            ViewBag.TotalPages = totalPages;
            ViewBag.Filter = request;

            return View(lessons);
        }

        [HttpGet]
        public async Task<IActionResult> Details(int id, int topicId, CancellationToken ct = default)
        {
            if (topicId <= 0)
            {
                return RedirectToAction("Index", "Language");
            }

            var lesson = await _lessonService.GetLessonByIdAsync(id, ct);
            if (lesson == null || lesson.TopicId != topicId)
            {
                TempData["NotFound"] = "Không tìm thấy bài học.";
                return RedirectToAction(nameof(Index), new { topicId });
            }

            ViewBag.TopicId = topicId;
            return View(lesson);
        }

        [HttpGet]
        public async Task<IActionResult> Create(int topicId, CancellationToken ct = default)
        {
            if (topicId <= 0)
            {
                return RedirectToAction("Index", "Language");
            }

            var topic = await _topicService.GetTopicByIdAsync(topicId, ct);
            if (topic == null)
            {
                TempData["NotFound"] = "Không tìm thấy chủ đề.";
                return RedirectToAction("Index", "Language");
            }

            ViewBag.TopicId = topicId;
            ViewBag.TopicName = topic.Name;
            ViewBag.LanguageId = topic.LanguageId;
            ViewBag.LanguageName = topic.LanguageName;

            return View(new CreateLessonRequest
            {
                TopicId = topicId
            });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(int topicId, CreateLessonRequest request, CancellationToken ct = default)
        {
            if (topicId <= 0)
            {
                return RedirectToAction("Index", "Language");
            }

            var topic = await _topicService.GetTopicByIdAsync(topicId, ct);
            if (topic == null)
            {
                TempData["NotFound"] = "Không tìm thấy chủ đề.";
                return RedirectToAction("Index", "Language");
            }

            if (!ModelState.IsValid)
            {
                ViewBag.TopicId = topicId;
                ViewBag.TopicName = topic.Name;
                ViewBag.LanguageId = topic.LanguageId;
                ViewBag.LanguageName = topic.LanguageName;
                return View(request);
            }

            request.TopicId = topicId;
            await _lessonService.CreateLessonAsync(request, ct);
            return RedirectToAction(nameof(Index), new { topicId = request.TopicId });
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id, int topicId, CancellationToken ct = default)
        {
            if (topicId <= 0)
            {
                return RedirectToAction("Index", "Language");
            }

            var lesson = await _lessonService.GetLessonByIdAsync(id, ct);
            if (lesson == null || lesson.TopicId != topicId)
            {
                TempData["NotFound"] = "Không tìm thấy bài học.";
                return RedirectToAction(nameof(Index), new { topicId });
            }

            ViewBag.TopicId = topicId;
            ViewBag.TopicName = lesson.TopicName;
            ViewBag.LanguageName = lesson.LanguageName;
            ViewBag.LessonId = id;

            var request = new UpdateLessonRequest
            {
                TopicId = lesson.TopicId,
                Title = lesson.Title,
                Description = lesson.Description,
                Content = lesson.Content
            };

            return View(request);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, int topicId, UpdateLessonRequest request, CancellationToken ct = default)
        {
            if (topicId <= 0)
            {
                return RedirectToAction("Index", "Language");
            }

            var lesson = await _lessonService.GetLessonByIdAsync(id, ct);
            if (lesson == null || lesson.TopicId != topicId)
            {
                TempData["NotFound"] = "Bài học không thuộc chủ đề này.";
                return RedirectToAction(nameof(Index), new { topicId });
            }

            if (!ModelState.IsValid)
            {
                ViewBag.TopicId = topicId;
                ViewBag.TopicName = lesson.TopicName;
                ViewBag.LanguageName = lesson.LanguageName;
                ViewBag.LessonId = id;
                return View(request);
            }

            request.TopicId = topicId;
            var updated = await _lessonService.UpdateLessonAsync(id, request, ct);
            if (!updated)
            {
                TempData["NotFound"] = "Không tìm thấy bài học.";
            }

            return RedirectToAction(nameof(Index), new { topicId = request.TopicId });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id, int topicId, CancellationToken ct = default)
        {
            if (topicId <= 0)
            {
                return RedirectToAction("Index", "Language");
            }

            var lesson = await _lessonService.GetLessonByIdAsync(id, ct);
            if (lesson == null || lesson.TopicId != topicId)
            {
                TempData["NotFound"] = "Bài học không thuộc chủ đề này.";
                return RedirectToAction(nameof(Index), new { topicId });
            }

            var deleted = await _lessonService.DeleteLessonAsync(id, ct);
            if (!deleted)
            {
                TempData["NotFound"] = "Không tìm thấy bài học.";
            }

            return RedirectToAction(nameof(Index), new { topicId });
        }
    }
}


