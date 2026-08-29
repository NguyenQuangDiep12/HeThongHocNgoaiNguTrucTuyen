namespace HeThongHocNgoaiNguTrucTuyen.Dtos.Responses
{
    public class QuestionInfoResponse
    {
        public int QuestionId { get; set; }

        public int TestId { get; set; }

        public string TestTitle { get; set; } = string.Empty;

        public string Content { get; set; } = string.Empty;

        public string? ImageUrl { get; set; }

        public int? PartNumber { get; set; }

        public string QuestionType { get; set; } = string.Empty;

        public string QuestionTypeDisplay { get; set; }
            = string.Empty;

        public int QuestionOrder { get; set; }

        public string? AudioUrl { get; set; }

        public string? GroupCode { get; set; }

        public int AnswerCount { get; set; }
    }
}