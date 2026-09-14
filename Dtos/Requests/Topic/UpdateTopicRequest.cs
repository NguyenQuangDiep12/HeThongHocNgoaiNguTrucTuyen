using System.ComponentModel.DataAnnotations;

namespace HeThongHocNgoaiNguTrucTuyen.Dtos.Requests.Topic
{
    public class UpdateTopicRequest
    {
        [Required(ErrorMessage = "Tên chủ đề không được bỏ trống")]
        [MaxLength(150, ErrorMessage = "Tên chủ đề không được vượt quá 150 ký tự")]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "Vui lòng chọn ngôn ngữ")]
        [Range(1, int.MaxValue, ErrorMessage = "Vui lòng chọn ngôn ngữ")]
        public int LanguageId { get; set; }

        [MaxLength(50, ErrorMessage = "Chiều dài cấp độ không vượt quá 50 ký tự")]
        public string? Level { get; set; }

        public string? Description { get; set; }

        public string? ImageUrl { get; set; }
    }
}
