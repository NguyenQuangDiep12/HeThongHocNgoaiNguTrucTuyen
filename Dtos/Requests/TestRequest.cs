using HeThongHocNgoaiNguTrucTuyen.Models.Enums;
using System.ComponentModel.DataAnnotations;

namespace HeThongHocNgoaiNguTrucTuyen.Dtos.Requests
{
    public class TestFilterRequest
    {
        public string? Title { get; set; }
    }
    public class CreateTestRequest
    {
        [Required(ErrorMessage = "Tên Bài kiểm tra không được để trống")]
        [MaxLength(200, ErrorMessage = "Tên bài học không được vượt quá 200 ký tự")]
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public int DurationMinutes { get; set; }
        public string TestMode { get; set; } = string.Empty;
    }
    public class UpdateTestRequest
    {
        [Required(ErrorMessage = "Tên Bài kiểm tra không được để trống")]
        [MaxLength(200, ErrorMessage = "Tên bài học không được vượt quá 200 ký tự")]
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public int DurationMinutes { get; set; }
        public string TestMode { get; set; } = string.Empty;
    }
}