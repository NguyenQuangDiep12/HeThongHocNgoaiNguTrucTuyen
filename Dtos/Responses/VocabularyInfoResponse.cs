namespace HeThongHocNgoaiNguTrucTuyen.Dtos.Responses
{
    public class VocabularyInfoResponse
    {
        public int VocabularyId { get; set; }
        public int LessonId { get; set; }
        public string Word { get; set; } = string.Empty;
        public string Meaning { get; set; } = string.Empty;
        public string? Phoenic { get; set; }
        public string? Example { get; set; }
        public string LessonTitle { get; set; } = string.Empty;
    }
}