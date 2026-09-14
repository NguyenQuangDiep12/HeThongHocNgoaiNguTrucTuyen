using System.ComponentModel.DataAnnotations;

namespace HeThongHocNgoaiNguTrucTuyen.Dtos.Requests
{
    public class LanguageFilterRequest
    {
        public string? Name { get; set; }
    }
    public class CreateLanguageRequest
    {
        [Required(ErrorMessage = "Yeu cau nhap vao ten ngon ngu")]
        [MaxLength(20)]
        public string Name { get; set; } = string.Empty;
        [Required(ErrorMessage = "Yeu cau nhap ma ngon ngu")]
        [MaxLength(10)]
        public string Code { get; set; } = string.Empty;
        public string? Description { get; set; } = string.Empty;
    }
    public class UpdateLanguageRequest
    {
        [Required(ErrorMessage = "Yeu cau nhap vao ten ngon ngu")]
        [MaxLength(20)]
        public string Name { get; set; } = string.Empty;
        [Required(ErrorMessage = "Yeu cau nhap ma ngon ngu")]
        [MaxLength(10)]
        public string Code { get; set; } = string.Empty;
        public string? Description { get; set; } = string.Empty;
    }
}
