namespace HeThongHocNgoaiNguTrucTuyen.Dtos.Responses
{
    public class LessonInfoResponse
    {
        public int LessonId { get; set; }
        public int TopicId { get; set; }
        public string Title { get; set; } = string.Empty;
        public string? Description { get; set; }
        public string? Content { get; set; }
        public string TopicName { get; set; } = string.Empty;
        public string LanguageName { get; set; } = string.Empty;
    }
}