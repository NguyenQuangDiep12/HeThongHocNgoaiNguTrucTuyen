namespace HeThongHocNgoaiNguTrucTuyen.Dtos.Responses
{
    public class TopicInfoResponse
    {
        public int TopicId { get; set; }
        public string Name { get; set; } = string.Empty;

        public string? Level { get; set; }

        public string? Description { get; set; }

        public string? ImageUrl { get; set; }

        public int LanguageId { get; set; }

        public string LanguageName { get; set; } = string.Empty;
    }
}
