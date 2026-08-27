namespace HeThongHocNgoaiNguTrucTuyen.Dtos.Responses
{
    public class LoginResponse
    {
        public int UserId { get; set; }
        public string FullName { get; set; } = string.Empty;
        public string RoleName { get; set; } = string.Empty;
    }
}
