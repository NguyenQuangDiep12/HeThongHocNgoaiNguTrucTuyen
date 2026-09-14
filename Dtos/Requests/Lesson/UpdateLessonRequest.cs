using System.ComponentModel.DataAnnotations;

namespace HeThongHocNgoaiNguTrucTuyen.Dtos.Requests.Lesson
{
    public class UpdateLessonRequest
    {
        [Required(ErrorMessage = "Vui lòng nhập tiêu đề")]
        [Range(1, int.MaxValue, ErrorMessage = "Vui lòng nhập tiêu đề")]
        public int TopicId { get; set; }
        [Required(ErrorMessage = "Tiêu đề bài học không được để trống")]
        [MaxLength(200)]
        public string Title { get; set; } = string.Empty;
        public string? Description { get; set; } = string.Empty;
        public string? Content { get; set; } = string.Empty;
    }
}
