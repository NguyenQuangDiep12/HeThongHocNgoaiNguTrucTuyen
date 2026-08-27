using HeThongHocNgoaiNguTrucTuyen.Dtos.Requests;
using HeThongHocNgoaiNguTrucTuyen.Dtos.Responses;
using HeThongHocNgoaiNguTrucTuyen.Services.Interfaces;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace HeThongHocNgoaiNguTrucTuyen.Controllers
{
    public class AuthController : Controller
    {
        private readonly IAuthService _authService;
        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterRequest request, CancellationToken ct)
        {
            if (!ModelState.IsValid)
            {
                return View(request);
            }
            try
            {
                bool isSuccess = await _authService.RegisterAsync(request, ct);
                if (!isSuccess)
                {
                    ModelState.AddModelError("","Đăng ký không thành công.");
                    return View(request);
                }

                TempData["RegisterSuccess"] = "Đăng ký thành công! Vui lòng đăng nhập vào hệ thống.";

                return RedirectToAction(nameof(Login));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View(request);
            }
        }
        [HttpGet]
        public IActionResult Login()
        {
            TempData.Remove("RegisterSuccess"); // xoa tempdata thong bao dang ky thanh cong chuyen thanh dang nhap
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginRequest request, CancellationToken ct)
        {
            if (!ModelState.IsValid)
            {
                return View(request);
            }

            try
            {
                // nhan thong tin userresponse
                LoginResponse? user = await _authService.LoginAsync(request, ct);

                // Xu ly cookie Authentication dang nhap vao he thong
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString()),
                    new Claim(ClaimTypes.Name, user.FullName),
                    new Claim(ClaimTypes.Role, user.RoleName)
                };

                var claimIdentity = new ClaimsIdentity(
                    claims, 
                    "cookie");


                // neu nhu nhan RememberMe tang thoi gian het han cua cookie
                var authProperties = new AuthenticationProperties
                {
                    IsPersistent = request.RememberMe,
                    ExpiresUtc = request.RememberMe ? DateTimeOffset.UtcNow.AddDays(3) : null
                };

                await HttpContext
                    .SignInAsync(
                    "cookie",
                    new ClaimsPrincipal(claimIdentity),
                    authProperties);

                // redirect sang trang home mac dinh khi dang nhap thanh cong
                return RedirectToAction("Index", "Home");
            }catch(Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View(request);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync("cookie");

            return RedirectToAction("Index", "Home");
        }

        // Tra ve 2 trang 404 hoac 403 Forbidden khi nguoi dung ko co role hop le hoac ko truy cap dung endpoint
        [HttpGet]
        public IActionResult Forbidden()
        {
            return View();
        }
    }
}
