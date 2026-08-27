using System.ComponentModel.DataAnnotations;

namespace HeThongHocNgoaiNguTrucTuyen.Models
{
    public class Vocabulary
    {
        public int VocabularyId { get; set; }

        public int LessonId { get; set; }

        public string Word { get; set; } = string.Empty;

        public string Meaning { get; set; } = string.Empty;

        public string? Example { get; set; }

        // FK -> Lesson
        public Lesson Lesson { get; set; } = null!;
    }
}
