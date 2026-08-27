using System.ComponentModel.DataAnnotations;

namespace HeThongHocNgoaiNguTrucTuyen.Models
{
    public class UserAnswer
    {
        public int UserAnswerId { get; set; }

        public int TestResultId { get; set; }

        public int QuestionId { get; set; }

        public int? AnswerId { get; set; }

        public bool IsCorrect { get; set; }

        // FK -> TestResult
        public TestResult TestResult { get; set; } = null!;

        // FK -> Question
        public Question Question { get; set; } = null!;

        // Nullable FK -> Answer
        public Answer? Answer { get; set; }
    }
}
