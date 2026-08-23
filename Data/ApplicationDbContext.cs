using Microsoft.EntityFrameworkCore;

namespace HeThongHocNgoaiNguTrucTuyen.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }

    }
}
