namespace HeThongHocNgoaiNguTrucTuyen.Models
{
    public class Language
    {
        public int LanguageId { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public string? Description { get; set; }
        // Bieu thi moi quan he lien ket thong qua navigation property
        public ICollection<Topic> Topics { get; set; } = new List<Topic>();
    }
}
