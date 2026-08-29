namespace HeThongHocNgoaiNguTrucTuyen.Dtos.Responses
{
    public class TestInfoResponse
    {
        public int TestId { get; set; }

        public string Title { get; set; } = string.Empty;

        public string? Description { get; set; }

        // 0 = PART
        // 1 = FULL
        public int TestMode { get; set; }

        public string TestModeDisplay { get; set; }
            = string.Empty;

        public int? PartNumber { get; set; }

        public int DurationMinutes { get; set; }

        public int QuestionCount { get; set; }
    }
}