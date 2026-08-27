using System.ComponentModel.DataAnnotations;

namespace HeThongHocNgoaiNguTrucTuyen.Models
{
    public class Answer
    {
        public int AnswerId { get; set; }

        public int QuestionId { get; set; }

        public string Content { get; set; } = string.Empty;

        public bool IsCorrect { get; set; }

        // FK -> Question
        public Question Question { get; set; } = null!;

        // 1 Answer - N UserAnswers
        public ICollection<UserAnswer> UserAnswers { get; set; } = new List<UserAnswer>();
    }
}
