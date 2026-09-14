using System.ComponentModel.DataAnnotations;

namespace HeThongHocNgoaiNguTrucTuyen.Dtos.Requests
{
    public class LessonFilterRequest
    {
        [Range(1, int.MaxValue, ErrorMessage = "Ngôn ngữ không được bỏ trống")]
        public int LanguageId { get; set; }
        [Range(1, int.MaxValue, ErrorMessage = "Tiêu đề không được để trống")]
        public int TopicId { get; set; }
        [MaxLength(200, ErrorMessage = "Tiêu đề không được vượt quá 200 ký tự")]
        public string? Title { get; set; }
    }
    public class CreateLessonRequest
    {
        [Required(ErrorMessage = "Ngôn ngữ không được để trống")]
        public int LanguageId { get; set; }
        [Required(ErrorMessage = "Chủ đề không được bỏ trống")]
        public int TopicId { get; set; }
        [Required(ErrorMessage = "Tiêu đề bài học không được để trống")]
        [MaxLength(200)]
        public string Title { get; set; } = string.Empty;
        public string? Description { get; set; } = string.Empty;
        public string? Content { get; set; } = string.Empty;
    }
    public class UpdateLessonRequest
    {
        [Required(ErrorMessage = "Tiêu đề bài học không được để trống")]
        [MaxLength(200)]
        public string Title { get; set; } = string.Empty;
        public string? Description { get; set; } = string.Empty;
        public string? Content { get; set; } = string.Empty;
    }
}