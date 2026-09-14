using HeThongHocNgoaiNguTrucTuyen.Dtos.Requests;
using HeThongHocNgoaiNguTrucTuyen.Services;
using HeThongHocNgoaiNguTrucTuyen.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HeThongHocNgoaiNguTrucTuyen.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "ADMIN")]
    public class VocabularyController : Controller
    {
        private readonly ILanguageService _languageService;
        private readonly ILessonService _lessonService;
        private readonly IVocabularyService _vocabularyService;

        public VocabularyController(ILanguageService languageService, ILessonService lessonService, IVocabularyService vocabularyService)
        {
            _languageService = languageService;
            _lessonService = lessonService;
            _vocabularyService = vocabularyService;
        }
        [HttpGet]
        public async Task<IActionResult> Index(VocabularyFilterRequest request, int pageNumber = 1, int pageSize = 10, CancellationToken ct = default)
        {
            pageNumber = pageNumber <= 0 ? 1 : pageNumber;
            pageSize = pageSize <= 0 ? 10 : Math.Min(pageSize, 10);

            var vocabularies = await _vocabularyService.GetVocabulariesAsync(request, pageNumber, pageSize, ct);

            var totalItems = await _vocabularyService.CountVocabulariesAsync(request, ct);

            var totalPages = (int)Math.Ceiling(
                totalItems / (double)pageSize
            );

            // Lấy danh sách bài học để đổ vào dropdown
            var lessons = await _lessonService.GetAllLessonsAsync(ct);

            ViewBag.Lessons = lessons;
            ViewBag.LessonId = request.LessonId;
            ViewBag.Word = request.Word;
            ViewBag.PageNumber = pageNumber;
            ViewBag.PageSize = pageSize;
            ViewBag.TotalPages = totalPages;

            return View(vocabularies);
        }
        [HttpGet]
        public async Task<IActionResult> Create(CancellationToken ct = default)
        {
            var languages = await _languageService.GetAllLanguagesAsync(ct);

            ViewBag.Languages = languages;
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(int lessonId, CreateVocabularyRequest request, CancellationToken ct = default)
        {
            if (!ModelState.IsValid)
            {
                return View(request);
            }
            await _vocabularyService.CreateVocabularyAsync(lessonId, request, ct);
            return RedirectToAction(nameof(Index));
        }
        [HttpGet]
        public async Task<IActionResult> Edit(int id, CancellationToken ct = default)
        {
            var vocabulary = await _vocabularyService.GetVocabularyByIdAsync(id, ct);
            if (vocabulary == null)
            {
                TempData["NotFound"] = "Không tìm thấy từ vựng.";
                return RedirectToAction(nameof(Index));
            }
            var request = new UpdateVocabularyRequest
            {
                Word = vocabulary.Word,
                Meaning = vocabulary.Meaning,
                Phoenic = vocabulary.Phoenic,
                Example = vocabulary.Example
            };

            return View(request);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, UpdateVocabularyRequest request, CancellationToken ct = default)
        {
            if (!ModelState.IsValid)
            {
                return View(request);
            }
            var updated = await _vocabularyService.UpdateVocabularyAsync(id, request, ct);
            if (!updated)
            {
                TempData["NotFound"] = "Không tìm thấy từ vựng.";
                return RedirectToAction(nameof(Index));
            }
            return RedirectToAction(nameof(Index));
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id, int lessonId, CancellationToken ct = default)
        {
            var deleted = await _vocabularyService.DeleteVocabularyAsync(id, ct);

            if (!deleted)
            {
                TempData["NotFound"] = "Không tìm thấy từ vựng.";
            }
            return RedirectToAction(nameof(Index),
                new
                {
                    lessonId = lessonId
                });
        }
    }
}
