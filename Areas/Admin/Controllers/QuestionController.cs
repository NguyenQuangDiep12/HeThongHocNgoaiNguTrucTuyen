using Microsoft.AspNetCore.Mvc;

namespace HeThongHocNgoaiNguTrucTuyen.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class QuestionController : Controller
    {
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }
    }
}
