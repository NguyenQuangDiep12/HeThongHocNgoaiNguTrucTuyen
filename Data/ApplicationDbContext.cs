using HeThongHocNgoaiNguTrucTuyen.Models;
using Microsoft.EntityFrameworkCore;

namespace HeThongHocNgoaiNguTrucTuyen.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }
        public DbSet<User> Users { get; set; }
        public DbSet<Answer> Answers { get; set; }
        public DbSet<UserAnswer> UserAnswers { get; set; }
        public DbSet<Vocabulary> Vocabularies { get; set; }
        public DbSet<Topic> Topics { get; set; }
        public DbSet<Test> Tests { get; set; }
        public DbSet<TestResult> TestResults { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Question> Questions { get; set; }
        public DbSet<Lesson> Lessons { get; set; }
        public DbSet<LearningProgress> LearningProgresses { get; set; }
        public DbSet<Language> Languages { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Quet qua toan bo project (Assembly) de tim cac cau hinh (Fluent API, DataAnnotation)....
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);

            modelBuilder.Entity<Role>().HasData(
                new Role
                {
                    RoleId = 1,
                    RoleName = "ADMIN",
                    Description = "Quan tri vien cua he thong"
                },
                new Role
                {
                    RoleId = 2,
                    RoleName = "USER",
                    Description = "Nguoi dung he thong"
                }
            );

            // Password Bich1234
            modelBuilder.Entity<User>().HasData(
                    new User
                    {
                        UserId = 5,
                        FullName = "Pham Van Ngoc",
                        Email = "NgocBich@gmail.com",
                        RoleId = 1,
                        PasswordHash = "$2a$11$TaR3tZiGkRlqKEk.aKh5IuEgOjjKkRfF9T./LQf685xRaVwTOQ0y6"
                    }
                );
        }
    }
}
