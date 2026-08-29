using HeThongHocNgoaiNguTrucTuyen.Dtos.Requests;
using HeThongHocNgoaiNguTrucTuyen.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace HeThongHocNgoaiNguTrucTuyen.Areas.Admin.Controllers
{
    [Area("Admin")]
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
        public async Task<IActionResult> Index(string? title, int? topicId, int pageNumber = 1, int pageSize = 10, CancellationToken ct = default)
        {
            if (pageNumber < 1)
            {
                pageNumber = 1;
            }

            if (pageSize < 1)
            {
                pageSize = 10;
            }

            // Lấy tổng số Lesson
            var totalItems = await _lessonService.CountLessonsAsync(title, topicId, ct);
            var totalPages = (int)Math.Ceiling(totalItems / (double)pageSize);

            // Nếu page hiện tại vượt quá tổng số trang
            if (totalPages > 0 && pageNumber > totalPages)
            {
                pageNumber = totalPages;
            }

            // Lấy Lesson
            var lessons = await _lessonService.GetLessonsAsync(title, topicId, pageNumber, pageSize, ct);

            ViewBag.Title = title;
            ViewBag.TopicId = topicId;
            ViewBag.PageNumber = pageNumber;
            ViewBag.PageSize = pageSize;
            ViewBag.TotalPages = totalPages;

            return View(lessons);
        }

        [HttpGet]
        public async Task<IActionResult> Details(
            int id,
            CancellationToken ct = default)
        {
            var lesson = await _lessonService.GetLessonByIdAsync(
                id,
                ct);

            if (lesson == null)
            {
                TempData["NotFound"] = "Không tìm thấy bài học.";

                return RedirectToAction(nameof(Index));
            }

            return View(lesson);
        }


        // =========================
        // CREATE - GET
        // =========================

        [HttpGet]
        public async Task<IActionResult> Create(
            CancellationToken ct = default)
        {
            var topics = await _topicService.GetTopicsAsync(
                100,
                1,
                new TopicRequest(),
                ct);

            ViewBag.Topics = topics;

            return View();
        }


        // =========================
        // CREATE - POST
        // =========================

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(
            LessonRequest request,
            CancellationToken ct = default)
        {
            if (!ModelState.IsValid)
            {
                var topics = await _topicService.GetTopicsAsync(
                    100,
                    1,
                    new TopicRequest(),
                    ct);

                ViewBag.Topics = topics;

                return View(request);
            }

            await _lessonService.CreateLessonAsync(
                request,
                ct);

            return RedirectToAction(nameof(Index));
        }


        // =========================
        // EDIT - GET
        // =========================

        [HttpGet]
        public async Task<IActionResult> Edit(
            int id,
            CancellationToken ct = default)
        {
            var lesson = await _lessonService.GetLessonByIdAsync(
                id,
                ct);

            if (lesson == null)
            {
                TempData["NotFound"] = "Không tìm thấy bài học.";

                return RedirectToAction(nameof(Index));
            }

            var topics = await _topicService.GetTopicsAsync(
                100,
                1,
                new TopicRequest(),
                ct);

            ViewBag.Topics = topics;

            var request = new LessonRequest
            {
                TopicId = lesson.TopicId,
                Title = lesson.Title,
                Description = lesson.Description,
                Content = lesson.Content
            };

            return View(request);
        }


        // =========================
        // EDIT - POST
        // =========================

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(
            int id,
            LessonRequest request,
            CancellationToken ct = default)
        {
            if (!ModelState.IsValid)
            {
                var topics = await _topicService.GetTopicsAsync(
                    100,
                    1,
                    new TopicRequest(),
                    ct);

                ViewBag.Topics = topics;

                return View(request);
            }

            var updated = await _lessonService.UpdateLessonAsync(
                id,
                request,
                ct);

            if (!updated)
            {
                TempData["NotFound"] = "Không tìm thấy bài học.";

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
            CancellationToken ct = default)
        {
            var deleted = await _lessonService.DeleteLessonAsync(
                id,
                ct);

            if (!deleted)
            {
                TempData["NotFound"] = "Không tìm thấy bài học.";
            }

            return RedirectToAction(nameof(Index));
        }
    }
}