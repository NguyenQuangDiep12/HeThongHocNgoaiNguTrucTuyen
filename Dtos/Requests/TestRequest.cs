using HeThongHocNgoaiNguTrucTuyen.Models.Enums;
using System.ComponentModel.DataAnnotations;

namespace HeThongHocNgoaiNguTrucTuyen.Dtos.Requests
{
    public class TestRequest
    {
        [Required(ErrorMessage = "Tên bài kiểm tra không được để trống.")]
        [StringLength(200, ErrorMessage = "Tên bài kiểm tra không được vượt quá 200 ký tự.")]
        public string Title { get; set; } = string.Empty;

        public string? Description { get; set; }

        public TestMode TestMode { get; set; }

        [Range(1, 7, ErrorMessage = "Part phải nằm trong khoảng từ 1 đến 7.")]
        public int? PartNumber { get; set; }

        [Range(1, 1000, ErrorMessage = "Thời gian làm bài phải lớn hơn 0.")]
        public int DurationMinutes { get; set; }
    }
}