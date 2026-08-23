using Microsoft.AspNetCore.Mvc;

namespace HeThongHocNgoaiNguTrucTuyen.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class TestController : Controller
    {
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }
    }
}
