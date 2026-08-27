using System.ComponentModel.DataAnnotations;

namespace HeThongHocNgoaiNguTrucTuyen.Models
{
    public class Topic
    {
        public int TopicId { get; set; }

        public int LanguageId { get; set; }

        public string Name { get; set; } = string.Empty;

        public string? Level { get; set; }

        public string? Description { get; set; }

        public string? ImageUrl { get; set; }

        // FK -> Language
        public Language Language { get; set; } = null!;

        // 1 Topic - N Lessons
        public ICollection<Lesson> Lessons { get; set; } = new List<Lesson>();
    }
}
