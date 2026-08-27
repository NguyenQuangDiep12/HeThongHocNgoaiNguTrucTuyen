using System.ComponentModel.DataAnnotations;

namespace HeThongHocNgoaiNguTrucTuyen.Models
{
    public class Lesson
    {
        public int LessonId { get; set; }

        public int TopicId { get; set; }

        public string Title { get; set; } = string.Empty;

        public string? Description { get; set; }

        public string? Content { get; set; }

        // FK -> Topic
        public Topic Topic { get; set; } = null!;

        // 1 Lesson - N Vocabulary
        public ICollection<Vocabulary> Vocabularies { get; set; } = new List<Vocabulary>();

        // 1 Lesson - N LearningProgress
        public ICollection<LearningProgress> LearningProgresses { get; set; } = new List<LearningProgress>();
    }
}
