using HeThongHocNgoaiNguTrucTuyen.Models.Enums;
using System.ComponentModel.DataAnnotations;

namespace HeThongHocNgoaiNguTrucTuyen.Models
{
    public class Question
    {
        public int QuestionId { get; set; }

        public int TestId { get; set; }

        public string Content { get; set; } = string.Empty;

        public string? ImageUrl { get; set; }

        public int? PartNumber { get; set; }

        public QuestionType QuestionType { get; set; }

        public int QuestionOrder { get; set; }

        public string? AudioUrl { get; set; }

        public string? GroupCode { get; set; }

        // FK -> Test
        public Test Test { get; set; } = null!;

        // 1 Question - N Answers
        public ICollection<Answer> Answers { get; set; } = new List<Answer>();

        // 1 Question - N UserAnswers
        public ICollection<UserAnswer> UserAnswers { get; set; } = new List<UserAnswer>();
    }
}
