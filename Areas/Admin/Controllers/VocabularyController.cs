using HeThongHocNgoaiNguTrucTuyen.Dtos.Requests.Vocabulary;
using HeThongHocNgoaiNguTrucTuyen.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HeThongHocNgoaiNguTrucTuyen.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "ADMIN")]
    public class VocabularyController : Controller
    {
        private readonly IVocabularyService _vocabularyService;
        private readonly ILessonService _lessonService;

        public VocabularyController(IVocabularyService vocabularyService, ILessonService lessonService)
        {
            _vocabularyService = vocabularyService;
            _lessonService = lessonService;
        }

        [HttpGet]
        public async Task<IActionResult> Index(int lessonId, VocabularyFilterRequest? request = null, int pageNumber = 1, int pageSize = 10, CancellationToken ct = default)
        {
            if (lessonId <= 0)
            {
                return RedirectToAction("Index", "Language");
            }

            request ??= new VocabularyFilterRequest();

            var lesson = await _lessonService.GetLessonByIdAsync(lessonId, ct);
            if (lesson == null)
            {
                TempData["NotFound"] = "Không tìm thấy bài học.";
                return RedirectToAction("Index", "Language");
            }

            var totalItems = await _vocabularyService.CountVocabulariesAsync(lessonId, request, ct);
            var totalPages = Math.Max(1, (int)Math.Ceiling((double)totalItems / (double)pageSize));
            if (pageNumber > totalPages)
            {
                pageNumber = totalPages;
            }

            var vocabularies = await _vocabularyService.GetVocabulariesAsync(lessonId, request, pageNumber, pageSize, ct);

            ViewBag.LessonId = lessonId;
            ViewBag.LessonTitle = lesson.Title;
            ViewBag.TopicId = lesson.TopicId;
            ViewBag.TopicName = lesson.TopicName;
            ViewBag.LanguageName = lesson.LanguageName;
            ViewBag.PageNumber = pageNumber;
            ViewBag.PageSize = pageSize;
            ViewBag.TotalPages = totalPages;
            ViewBag.Filter = request;

            return View(vocabularies);
        }

        [HttpGet]
        public async Task<IActionResult> Create(int lessonId, CancellationToken ct = default)
        {
            if (lessonId <= 0)
            {
                return RedirectToAction("Index", "Language");
            }

            var lesson = await _lessonService.GetLessonByIdAsync(lessonId, ct);
            if (lesson == null)
            {
                TempData["NotFound"] = "Không tìm thấy bài học.";
                return RedirectToAction("Index", "Language");
            }

            ViewBag.LessonId = lessonId;
            ViewBag.LessonTitle = lesson.Title;
            ViewBag.TopicId = lesson.TopicId;
            ViewBag.TopicName = lesson.TopicName;
            ViewBag.LanguageName = lesson.LanguageName;

            return View(new CreateVocabularyRequest
            {
                LessonId = lessonId
            });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(int lessonId, CreateVocabularyRequest request, CancellationToken ct = default)
        {
            if (lessonId <= 0)
            {
                return RedirectToAction("Index", "Language");
            }

            var lesson = await _lessonService.GetLessonByIdAsync(lessonId, ct);
            if (lesson == null)
            {
                TempData["NotFound"] = "Không tìm thấy bài học.";
                return RedirectToAction("Index", "Language");
            }

            if (!ModelState.IsValid)
            {
                ViewBag.LessonId = lessonId;
                ViewBag.LessonTitle = lesson.Title;
                ViewBag.TopicId = lesson.TopicId;
                ViewBag.TopicName = lesson.TopicName;
                ViewBag.LanguageName = lesson.LanguageName;
                return View(request);
            }

            request.LessonId = lessonId;
            await _vocabularyService.CreateVocabularyAsync(request, ct);
            return RedirectToAction(nameof(Index), new { lessonId = request.LessonId });
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id, int lessonId, CancellationToken ct = default)
        {
            if (lessonId <= 0)
            {
                return RedirectToAction("Index", "Language");
            }

            var response = await _vocabularyService.GetVocabularyByIdAsync(id, ct);
            if (response == null || response.LessonId != lessonId)
            {
                TempData["NotFound"] = "Không tìm thấy từ vựng.";
                return RedirectToAction(nameof(Index), new { lessonId });
            }

            ViewBag.LessonId = lessonId;
            ViewBag.LessonTitle = response.LessonTitle;
            ViewBag.VocabularyId = id;

            var request = new UpdateVocabularyRequest
            {
                LessonId = response.LessonId,
                Word = response.Word,
                Meaning = response.Meaning,
                Phoenic = response.Phoenic ?? string.Empty,
                Example = response.Example
            };

            return View(request);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, int lessonId, UpdateVocabularyRequest request, CancellationToken ct = default)
        {
            if (lessonId <= 0)
            {
                return RedirectToAction("Index", "Language");
            }

            var response = await _vocabularyService.GetVocabularyByIdAsync(id, ct);
            if (response == null || response.LessonId != lessonId)
            {
                TempData["NotFound"] = "Từ vựng không thuộc bài học này.";
                return RedirectToAction(nameof(Index), new { lessonId });
            }

            if (!ModelState.IsValid)
            {
                ViewBag.LessonId = lessonId;
                ViewBag.LessonTitle = response.LessonTitle;
                ViewBag.VocabularyId = id;
                return View(request);
            }

            request.LessonId = lessonId;
            var updated = await _vocabularyService.UpdateVocabularyAsync(id, request, ct);
            if (!updated)
            {
                TempData["NotFound"] = "Không tìm thấy từ vựng.";
            }

            return RedirectToAction(nameof(Index), new { lessonId = request.LessonId });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id, int lessonId, CancellationToken ct = default)
        {
            if (lessonId <= 0)
            {
                return RedirectToAction("Index", "Language");
            }

            var response = await _vocabularyService.GetVocabularyByIdAsync(id, ct);
            if (response == null || response.LessonId != lessonId)
            {
                TempData["NotFound"] = "Từ vựng không thuộc bài học này.";
                return RedirectToAction(nameof(Index), new { lessonId });
            }

            var deleted = await _vocabularyService.DeleteVocabularyAsync(id, ct);
            if (!deleted)
            {
                TempData["NotFound"] = "Không tìm thấy từ vựng.";
            }

            return RedirectToAction(nameof(Index), new { lessonId });
        }
    }
}



