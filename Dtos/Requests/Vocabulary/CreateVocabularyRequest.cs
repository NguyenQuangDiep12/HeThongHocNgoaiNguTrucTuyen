using System.ComponentModel.DataAnnotations;

namespace HeThongHocNgoaiNguTrucTuyen.Dtos.Requests.Vocabulary
{
    public class CreateVocabularyRequest
    {
        [Required(ErrorMessage = "Bài học không được để trống")]
        public int LessonId { get; set; }
        [Required(ErrorMessage = "Từ vựng không được để trống")]
        [MaxLength(100, ErrorMessage = "Từ vựng không được vượt quá 100 ký tự")]
        public string Word { get; set; } = string.Empty;
        [Required(ErrorMessage = "Nghĩa không được để trống")]
        [MaxLength(200, ErrorMessage = "Nghĩa không được vượt quá 200 ký tự")]
        public string Meaning { get; set; } = string.Empty;
        [Required(ErrorMessage = "Phiên âm không được để trống")]
        [MaxLength(30, ErrorMessage = "Phiên âm không vượt quá 30 ký tự")]
        public string Phoenic { get; set; } = string.Empty;
        public string? Example { get; set; } = string.Empty;
    }
}
