using HeThongHocNgoaiNguTrucTuyen.Dtos.Requests;
using HeThongHocNgoaiNguTrucTuyen.Services;
using HeThongHocNgoaiNguTrucTuyen.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace HeThongHocNgoaiNguTrucTuyen.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "ADMIN")]
    public class LanguageController : Controller
    {
        private readonly ILanguageService _languageService;
        public LanguageController(ILanguageService languageService)
        {
            _languageService = languageService;
        }
        [HttpGet]
        public async Task<IActionResult> Index(int pageSize = 10, int pageNumber = 1, string? name = null, CancellationToken ct = default)
        {
            var response = await _languageService.GetLanguagesAsync(pageSize, pageNumber, name ?? string.Empty, ct);

            int languageCount = await _languageService.CountLanguagesAsync(name, ct);

            ViewBag.PageNumber = pageNumber;
            ViewBag.PageSize = pageSize;
            ViewBag.Name = name ?? string.Empty;
            ViewBag.TotalPages = (int)Math.Ceiling((decimal)languageCount / (decimal)pageSize);

            return View(response);
        }
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(LanguageRequest request, CancellationToken ct)
        {
            if (!ModelState.IsValid)
            {
                return View(request);
            }

            await _languageService.CreateLanguagesAsync(request, ct);
            return RedirectToAction(nameof(Index));
        }
        [HttpGet]
        public async Task<IActionResult> Edit(int id, CancellationToken ct)
        {
            var response = await _languageService.GetLanguageByIdAsync(id, ct);

            if (response == null)
            {
                TempData["NotFound"] = "Ngon ngu khong duoc tim thay";

                return RedirectToAction(nameof(Index));
            }
            ViewBag.LanguageId = id;
            return View(response);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, LanguageRequest request, CancellationToken ct)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.LanguageId = id;
                return View(request);
            }
            var language = await _languageService.UpdateLanguagesAsync(id, request, ct);
            if (!language)
            {
                TempData["NotFound"] = "Ngon ngu khong ton tai";
                return RedirectToAction(nameof(Index));
            }
            return RedirectToAction(nameof(Index));
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id, CancellationToken ct)
        {
            var response = await _languageService.DeleteLanguagesAsync(id, ct);
            if (!response)
            {
                TempData["NotFound"] = "Khong tim thay ngon ngu can xoa";
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
