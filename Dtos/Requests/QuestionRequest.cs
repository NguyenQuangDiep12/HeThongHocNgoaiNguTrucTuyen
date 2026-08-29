using System.ComponentModel.DataAnnotations;

namespace HeThongHocNgoaiNguTrucTuyen.Dtos.Requests
{
    public class QuestionRequest
    {
        [Required(ErrorMessage = "Vui lòng chọn bài kiểm tra.")]
        public int TestId { get; set; }

        [Required(ErrorMessage = "Nội dung câu hỏi không được để trống.")]
        public string Content { get; set; } = string.Empty;

        public string? ImageUrl { get; set; }

        [Range(1, 7, ErrorMessage = "Part phải từ 1 đến 7.")]
        public int? PartNumber { get; set; }

        [Range(1, int.MaxValue,
            ErrorMessage = "Thứ tự câu hỏi phải lớn hơn 0.")]
        public int QuestionOrder { get; set; }

        public string? AudioUrl { get; set; }

        public string? GroupCode { get; set; }
    }
}