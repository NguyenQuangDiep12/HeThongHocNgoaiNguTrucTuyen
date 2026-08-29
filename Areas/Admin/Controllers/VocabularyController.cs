using HeThongHocNgoaiNguTrucTuyen.Dtos.Requests;
using HeThongHocNgoaiNguTrucTuyen.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace HeThongHocNgoaiNguTrucTuyen.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class VocabularyController : Controller
    {
        private readonly IVocabularyService _vocabularyService;
        private readonly ILessonService _lessonService;
        public VocabularyController(
        IVocabularyService vocabularyService,
        ILessonService lessonService)
        {
            _vocabularyService = vocabularyService;
            _lessonService = lessonService;
        }

        // =========================
        // INDEX
        // =========================

        [HttpGet]
        public async Task<IActionResult> Index(
            string? word,
            int? lessonId,
            int pageNumber = 1,
            int pageSize = 10,
            CancellationToken ct = default)
        {
            // Chuẩn hóa dữ liệu phân trang
            if (pageNumber < 1)
            {
                pageNumber = 1;
            }

            if (pageSize < 1)
            {
                pageSize = 10;
            }

            // Đếm tổng số từ vựng
            var totalItems =
                await _vocabularyService.CountVocabulariesAsync(
                    word,
                    lessonId,
                    ct);

            // Tính tổng số trang
            var totalPages =
                (int)Math.Ceiling(
                    totalItems / (double)pageSize);

            // Nếu không có dữ liệu vẫn giữ trang 1
            if (totalPages < 1)
            {
                totalPages = 1;
            }

            // Nếu trang hiện tại lớn hơn tổng số trang
            if (pageNumber > totalPages)
            {
                pageNumber = totalPages;
            }

            // Lấy danh sách từ vựng
            var vocabularies =
                await _vocabularyService.GetVocabulariesAsync(
                    word,
                    lessonId,
                    pageNumber,
                    pageSize,
                    ct);

            // Lấy danh sách bài học cho dropdown
            // Không phân trang để tránh dropdown bị thiếu dữ liệu
            var lessons =
                await _lessonService.GetLessonsAsync(
                    null,
                    null,
                    1,
                    1000,
                    ct);

            ViewBag.Word = word;
            ViewBag.LessonId = lessonId;
            ViewBag.PageNumber = pageNumber;
            ViewBag.PageSize = pageSize;
            ViewBag.TotalPages = totalPages;
            ViewBag.Lessons = lessons;

            return View(vocabularies);
        }


        // =========================
        // CREATE - GET
        // =========================

        [HttpGet]
        public async Task<IActionResult> Create(
            int? lessonId,
            CancellationToken ct = default)
        {
            // Lấy danh sách Lesson cho dropdown
            var lessons =
                await _lessonService.GetLessonsAsync(
                    null,
                    null,
                    1,
                    1000,
                    ct);

            ViewBag.Lessons = lessons;

            // Nếu người dùng đi từ trang Lesson sang Vocabulary
            // thì tự động chọn LessonId
            var request = new VocabularyRequest
            {
                LessonId = lessonId ?? 0
            };

            return View(request);
        }


        // =========================
        // CREATE - POST
        // =========================

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(
            VocabularyRequest request,
            CancellationToken ct = default)
        {
            if (!ModelState.IsValid)
            {
                // Khi validation lỗi phải load lại dropdown Lesson
                var lessons =
                    await _lessonService.GetLessonsAsync(
                        null,
                        null,
                        1,
                        1000,
                        ct);

                ViewBag.Lessons = lessons;

                return View(request);
            }

            await _vocabularyService.CreateVocabularyAsync(
                request,
                ct);

            // Sau khi thêm xong quay về danh sách
            // và giữ lại Lesson hiện tại
            return RedirectToAction(
                nameof(Index),
                new
                {
                    lessonId = request.LessonId
                });
        }


        // =========================
        // EDIT - GET
        // =========================

        [HttpGet]
        public async Task<IActionResult> Edit(
            int id,
            CancellationToken ct = default)
        {
            // Lấy Vocabulary cần sửa
            var vocabulary =
                await _vocabularyService.GetVocabularyByIdAsync(
                    id,
                    ct);

            if (vocabulary == null)
            {
                TempData["NotFound"] =
                    "Không tìm thấy từ vựng.";

                return RedirectToAction(nameof(Index));
            }

            // Lấy Lesson cho dropdown
            var lessons =
                await _lessonService.GetLessonsAsync(
                    null,
                    null,
                    1,
                    1000,
                    ct);

            ViewBag.Lessons = lessons;

            // Chuyển VocabularyInfoResponse
            // sang VocabularyRequest để binding form
            var request = new VocabularyRequest
            {
                LessonId = vocabulary.LessonId,
                Word = vocabulary.Word,
                Meaning = vocabulary.Meaning,
                Phoenic = vocabulary.Phoenic,
                Example = vocabulary.Example
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
            VocabularyRequest request,
            CancellationToken ct = default)
        {
            if (!ModelState.IsValid)
            {
                // Load lại Lesson khi validation lỗi
                var lessons =
                    await _lessonService.GetLessonsAsync(
                        null,
                        null,
                        1,
                        1000,
                        ct);

                ViewBag.Lessons = lessons;

                return View(request);
            }

            var updated =
                await _vocabularyService.UpdateVocabularyAsync(
                    id,
                    request,
                    ct);

            if (!updated)
            {
                TempData["NotFound"] =
                    "Không tìm thấy từ vựng.";

                return RedirectToAction(nameof(Index));
            }

            return RedirectToAction(
                nameof(Index),
                new
                {
                    lessonId = request.LessonId
                });
        }


        // =========================
        // DELETE
        // =========================

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(
            int id,
            int lessonId,
            CancellationToken ct = default)
        {
            var deleted =
                await _vocabularyService.DeleteVocabularyAsync(
                    id,
                    ct);

            if (!deleted)
            {
                TempData["NotFound"] =
                    "Không tìm thấy từ vựng.";
            }

            return RedirectToAction(
                nameof(Index),
                new
                {
                    lessonId = lessonId
                });
        }
    }
}
