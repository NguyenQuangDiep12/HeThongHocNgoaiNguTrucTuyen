using System.ComponentModel.DataAnnotations;

namespace HeThongHocNgoaiNguTrucTuyen.Dtos.Requests
{
    public class RegisterRequest
    {
        [Required(ErrorMessage = "Ten nguoi dung khong duoc de trong")]
        public string FullName { get; set; } = string.Empty;
        [Required(ErrorMessage = "Email khong duoc bo trong!")]
        [EmailAddress(ErrorMessage = "Dia chi email khong hop le")]
        public string Email { get; set; } = string.Empty;
        [Required(ErrorMessage = "Mat khau khong duoc bo trong")]
        public string Password { get; set; } = string.Empty;
        [Required(ErrorMessage = "Xac nhan mat khau khong duoc bo trong")]
        [Compare(nameof(Password), ErrorMessage = "Xac nhan mat khau khong khop")]
        public string ConfirmPassword { get; set; } = string.Empty;
    }
}
