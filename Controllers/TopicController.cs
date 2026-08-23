using Microsoft.AspNetCore.Mvc;

namespace HeThongHocNgoaiNguTrucTuyen.Controllers
{
    public class TopicController : Controller
    {
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }
    }
}
