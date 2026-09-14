using HeThongHocNgoaiNguTrucTuyen.Dtos.Requests;
using HeThongHocNgoaiNguTrucTuyen.Models.Enums;
using HeThongHocNgoaiNguTrucTuyen.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HeThongHocNgoaiNguTrucTuyen.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "ADMIN")]
    public class TestController : Controller
    {
        private readonly ITestService _testService;

        public TestController(
            ITestService testService)
        {
            _testService = testService;
        }
        [HttpGet]
        public async Task<IActionResult> Index(TestFilterRequest request, int pageSize = 10, int pageNumber = 1, CancellationToken ct = default)
        {
            var tests = await _testService.GetTestsAsync(pageSize, pageNumber, request, ct);
            var totalCount = await _testService.CountTestsAsync(request, ct);
            ViewBag.PageNumber = pageNumber;
            ViewBag.PageSize = pageSize;
            ViewBag.TotalPages = Math.Max(1, (int)Math.Ceiling((decimal)totalCount / pageSize));
            ViewBag.Title = request.Title;
            return View(tests);
        }
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateTestRequest request, CancellationToken ct)
        {
            if (!ModelState.IsValid)
            {
                return View(request);
            }

            await _testService.CreateTestAsync(request, ct);
            TempData["Success"] = "Tạo bài kiểm tra thành công.";
            return RedirectToAction(nameof(Index));
        }
        [HttpGet]
        public async Task<IActionResult> Edit(int id, CancellationToken ct)
        {
            var test = await _testService.GetTestByIdAsync(id, ct);
            if (test == null)
            {
                TempData["NotFound"] = "Không tìm thấy bài kiểm tra.";
                return RedirectToAction(nameof(Index));
            }
            ViewBag.TestId = id;
            return View(new UpdateTestRequest
            {
                Title = test.Title,
                Description = test.Description,
                TestMode = test.TestMode,
                DurationMinutes = test.DurationMinutes
            });
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, UpdateTestRequest request, CancellationToken ct)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.TestId = id;
                return View(request);
            }
           
            try
            {
                var updated = await _testService.UpdateTestAsync(id, request, ct);

                return RedirectToAction(nameof(Index));
            }catch(Exception ex)
            {
                TempData["Error"] = ex.Message;
                return RedirectToAction(nameof(Index));
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id, CancellationToken ct)
        {
            try
            {
                var result = await _testService.DeleteTestAsync(id, ct);
                if (!result)
                {
                    TempData["NotFound"] = "Không tìm thấy bài kiểm tra cần xóa.";
                }
            }
            catch (InvalidOperationException ex)
            {
                TempData["Error"] = ex.Message;
            }
            return RedirectToAction(nameof(Index));
        }
    }
}