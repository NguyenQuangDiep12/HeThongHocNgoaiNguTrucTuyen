using HeThongHocNgoaiNguTrucTuyen.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HeThongHocNgoaiNguTrucTuyen.Data.Configurations
{
    public class TestConfiguration : IEntityTypeConfiguration<Test>
    {
        public void Configure(EntityTypeBuilder<Test> builder)
        {
            // Table
            builder.ToTable("Tests");

            // Primary Key
            builder.HasKey(t => t.TestId);

            builder.Property(t => t.TestId)
                .ValueGeneratedOnAdd();

            // Title
            builder.Property(t => t.Title)
                .IsRequired()
                .HasMaxLength(200);

            // Description
            builder.Property(t => t.Description)
                .IsRequired(false)
                .HasMaxLength(1000);

            // DurationMinutes
            builder.Property(t => t.DurationMinutes)
                .IsRequired();

            // TestMode
            builder.Property(t => t.TestMode)
                .IsRequired()
                .HasConversion<int>();

            // PartNumber
            builder.Property(t => t.PartNumber)
                .IsRequired(false);

            // Test 1 - N Question
            builder.HasMany(t => t.Questions)
                .WithOne(q => q.Test)
                .HasForeignKey(q => q.TestId)
                .OnDelete(DeleteBehavior.Cascade);

            // Test 1 - N TestResult
            builder.HasMany(t => t.TestResults)
                .WithOne(tr => tr.Test)
                .HasForeignKey(tr => tr.TestId)
                .OnDelete(DeleteBehavior.Restrict);

            // Check DurationMinutes > 0
            builder.ToTable("Tests", table =>
            {
                table.HasCheckConstraint(
                    "CK_Tests_DurationMinutes",
                    "[DurationMinutes] >= 0"
                );

                // Rang buoc cho tung dang test (TestMode)
                /*
                 FullTest: PartNumber == null
                 PartTest: PartNumber > 0
                 */
                table.HasCheckConstraint(
                    "CK_Tests_PartNumber",
                    "[PartNumber] IS NULL OR [PartNumber] > 0"
                );
            });
        }
    }
}