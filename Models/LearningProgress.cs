using HeThongHocNgoaiNguTrucTuyen.Models.Enums;
using System.ComponentModel.DataAnnotations;

namespace HeThongHocNgoaiNguTrucTuyen.Models
{
    public class LearningProgress
    {
        public int ProgressId { get; set; }

        public int UserId { get; set; }

        public int LessonId { get; set; }

        public LearningStatus Status { get; set; }

        public decimal CompletionPercent { get; set; }

        // FK -> User
        public User User { get; set; } = null!;

        // FK -> Lesson
        public Lesson Lesson { get; set; } = null!;
    }
}
