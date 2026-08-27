using System.ComponentModel.DataAnnotations;

namespace HeThongHocNgoaiNguTrucTuyen.Models
{
    public class TestResult
    {
        public int TestResultId { get; set; }

        public int UserId { get; set; }

        public int TestId { get; set; }

        public decimal Score { get; set; }

        public int CorrectCount { get; set; }

        public int TotalQuestion { get; set; }

        public DateTime SubmittedAt { get; set; }

        // FK -> User
        public User User { get; set; } = null!;

        // FK -> Test
        public Test Test { get; set; } = null!;

        // 1 TestResult - N UserAnswers
        public ICollection<UserAnswer> UserAnswers { get; set; } = new List<UserAnswer>();
    }
}
