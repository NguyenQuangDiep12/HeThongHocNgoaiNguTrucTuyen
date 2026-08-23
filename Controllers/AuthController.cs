using Microsoft.AspNetCore.Mvc;

namespace HeThongHocNgoaiNguTrucTuyen.Controllers
{
    public class AuthController : Controller
    {
        public AuthController()
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Register()
        {

        }
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }
    }
}
