using HeThongHocNgoaiNguTrucTuyen.Dtos.Requests;
using HeThongHocNgoaiNguTrucTuyen.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HeThongHocNgoaiNguTrucTuyen.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "ADMIN")]
    public class LessonController : Controller
    {
        private readonly ILanguageService _languageService;
        private readonly ILessonService _lessonService;
        public LessonController(ILanguageService languageService, ILessonService lessonService)
        {
            _languageService = languageService;
            _lessonService = lessonService;
        }

        [HttpGet]
        public async Task<IActionResult> Index(LessonFilterRequest request, int pageNumber = 1, int pageSize = 10, CancellationToken ct = default)
        {
            // Lấy Lesson
            var lessons = await _lessonService.GetLessonsAsync(request, pageNumber, pageSize, ct);
            // Lấy tổng số Lesson
            var totalItems = await _lessonService.CountLessonsAsync(request ,ct);
            var totalPages = (int)Math.Ceiling(totalItems / (double)pageSize);

            ViewBag.PageNumber = pageNumber;
            ViewBag.PageSize = pageSize;
            ViewBag.TotalPages = totalPages;
            return View(lessons);
        }

        [HttpGet]
        public async Task<IActionResult> Detail(int id, CancellationToken ct = default)
        {
            var lesson = await _lessonService.GetLessonByIdAsync(id, ct);
            if (lesson == null)
            {
                TempData["NotFound"] = "Không tìm thấy bài học.";
                return RedirectToAction(nameof(Index));
            }
            return View(lesson);
        }

        [HttpGet]
        public async Task<IActionResult> Create(CancellationToken ct = default)
        {
            var languages = await _languageService.GetAllLanguagesAsync(ct);

            ViewBag.Languages = languages;
            return View(new CreateLessonRequest());
        }

        [HttpGet]
        public async Task<IActionResult> GetLessonsByTopicIdDropdown(int topicId, CancellationToken ct = default)
        {
            var lessons = await _lessonService.GetLessonsByTopicIdAsync(topicId, ct);

            var result = lessons.Select(t => new
            {
                LessonId = t.LessonId,
                Title = t.Title,
            }).ToList();

            return Json(result);
            
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateLessonRequest request, CancellationToken ct = default)
        {
            if (!ModelState.IsValid)
            {
                var language = await _languageService.GetAllLanguagesAsync(ct);
                ViewBag.Languages = language;
                return View(request);
            }
            await _lessonService.CreateLessonAsync(request, ct);
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id, CancellationToken ct = default)
        {
            var lesson = await _lessonService.GetLessonByIdAsync(id, ct);
            if (lesson == null)
            {
                TempData["NotFound"] = "Không tìm thấy bài học.";
                return RedirectToAction(nameof(Index));
            }
            var request = new UpdateLessonRequest
            {
                Title = lesson.Title,
                Description = lesson.Description,
                Content = lesson.Content
            };

            return View(request);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, UpdateLessonRequest request, CancellationToken ct = default)
        {
            if (!ModelState.IsValid)
            {
                return View(request);
            }
            try
            {
                var updated = await _lessonService.UpdateLessonAsync(id, request, ct);

                return RedirectToAction(nameof(Index));
            }catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
                return RedirectToAction(nameof(Index));
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id, CancellationToken ct = default)
        {
            try
            {
                var deleted = await _lessonService.DeleteLessonAsync(id, ct);
                return RedirectToAction(nameof(Index));
            }catch(Exception ex)
            {
                TempData["Error"] = ex.Message;
                return RedirectToAction(nameof(Index));
            }
        }
    }
}