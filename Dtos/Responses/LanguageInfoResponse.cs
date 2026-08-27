namespace HeThongHocNgoaiNguTrucTuyen.Dtos.Responses
{
    public class LanguageInfoResponse
    {
        public int? LanguageId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Code { get; set; } = string.Empty;
        public string? Description { get; set; } = string.Empty;
    }

}
