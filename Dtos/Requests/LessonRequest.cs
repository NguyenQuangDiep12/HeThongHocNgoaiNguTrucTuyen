using System.ComponentModel.DataAnnotations;

namespace HeThongHocNgoaiNguTrucTuyen.Dtos.Requests
{
    public class LessonRequest
    {
        [Required(ErrorMessage = "Chủ đề không được để trống")]
        public int TopicId { get; set; }

        [Required(ErrorMessage = "Tiêu đề bài học không được để trống")]
        [MaxLength(200)]
        public string Title { get; set; } = string.Empty;

        public string? Description { get; set; }

        public string? Content { get; set; }
    }
}