using System.ComponentModel.DataAnnotations;

namespace HeThongHocNgoaiNguTrucTuyen.Dtos.Requests
{
    public class LoginRequest
    {
        [Required(ErrorMessage = "Email khong duoc bo trong!")]
        [EmailAddress(ErrorMessage = "Dia chi email khong hop le")]
        public string Email { get; set; } = string.Empty;
        [Required(ErrorMessage = "Mat khau khong duoc bo trong")]
        public string Password { get; set; } = string.Empty;
        public bool RememberMe { get; set; } = false;
    }
}
