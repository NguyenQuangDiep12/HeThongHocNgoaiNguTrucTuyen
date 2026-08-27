using HeThongHocNgoaiNguTrucTuyen.Models.Enums;
using System.ComponentModel.DataAnnotations;

namespace HeThongHocNgoaiNguTrucTuyen.Models
{
    public class Test
    {
        public int TestId { get; set; }

        public string Title { get; set; } = string.Empty;

        public string? Description { get; set; }

        public int DurationMinutes { get; set; }

        public TestMode TestMode { get; set; }

        public int? PartNumber { get; set; }

        // 1 Test - N Questions
        public ICollection<Question> Questions { get; set; } = new List<Question>();

        // 1 Test - N TestResults
        public ICollection<TestResult> TestResults { get; set; } = new List<TestResult>();
    }
}
