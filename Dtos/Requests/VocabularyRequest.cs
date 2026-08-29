using System.ComponentModel.DataAnnotations;

namespace HeThongHocNgoaiNguTrucTuyen.Dtos.Requests
{
    public class VocabularyRequest
    {
        [Required(ErrorMessage = "Bài học không được để trống")]
        public int LessonId { get; set; }
        [Required(ErrorMessage = "Từ vựng không được để trống")]
        [MaxLength(100)]
        public string Word { get; set; } = string.Empty;
        [Required(ErrorMessage = "Nghĩa không được để trống")]
        [MaxLength(200)]
        public string Meaning { get; set; } = string.Empty;
        [MaxLength(30)]
        public string? Phoenic { get; set; }
        public string? Example { get; set; }
    }
}