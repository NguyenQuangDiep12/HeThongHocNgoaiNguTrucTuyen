using System.ComponentModel.DataAnnotations;

namespace HeThongHocNgoaiNguTrucTuyen.Models
{
    public class User
    {
        public int UserId { get; set; }

        public string FullName { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;

        public string PasswordHash { get; set; } = string.Empty;

        public int RoleId { get; set; }

        // FK -> Role
        public Role Role { get; set; } = null!;

        // 1 User - N LearningProgress
        public ICollection<LearningProgress> LearningProgresses { get; set; } = new List<LearningProgress>();

        // 1 User - N TestResult
        public ICollection<TestResult> TestResults { get; set; } = new List<TestResult>();
    }
}
