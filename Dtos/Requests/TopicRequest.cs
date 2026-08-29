using System.ComponentModel.DataAnnotations;

namespace HeThongHocNgoaiNguTrucTuyen.Dtos.Requests
{
    public class TopicRequest
    {
        [Required(ErrorMessage = "Tên chủ đề không được bỏ trống")]
        [MaxLength(20, ErrorMessage = "Chiều dài tên không vượt quá 20 ký tự")]
        public string Name { get; set; } = string.Empty;
        [MaxLength(10, ErrorMessage = "Chiều dài cấp độ không vượt quá 10 ký tự")]
        public string Level { get; set; } = string.Empty;

        public string? Description { get; set; }

        public string? ImageUrl { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Vui lòng chọn ngôn ngữ")]
        public int LanguageId { get; set; }

        public string LanguageName { get; set; } = string.Empty;
    }
}
