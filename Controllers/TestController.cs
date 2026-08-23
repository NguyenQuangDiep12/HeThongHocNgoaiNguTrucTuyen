using Microsoft.AspNetCore.Mvc;

namespace HeThongHocNgoaiNguTrucTuyen.Controllers
{

    public class TestController : Controller
    {
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }
    }
}
