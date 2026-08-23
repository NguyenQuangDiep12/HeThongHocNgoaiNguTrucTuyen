using System.ComponentModel.DataAnnotations;

namespace HeThongHocNgoaiNguTrucTuyen.Models
{
    public class Role
    {
        [Key]
        public int RoleId { get; set; }

        public string RoleName { get; set; } = string.Empty;

        public string? Description { get; set; }

        // 1 Role - N Users
        public ICollection<User> Users { get; set; } = new List<User>();
    }
}
