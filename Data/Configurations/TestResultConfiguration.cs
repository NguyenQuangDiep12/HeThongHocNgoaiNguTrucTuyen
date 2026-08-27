using HeThongHocNgoaiNguTrucTuyen.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HeThongHocNgoaiNguTrucTuyen.Data.Configurations
{
    public class TestResultConfiguration : IEntityTypeConfiguration<TestResult>
    {
        public void Configure(EntityTypeBuilder<TestResult> builder)
        {
            // Table
            builder.ToTable("TestResults");

            // Primary Key
            builder.HasKey(tr => tr.TestResultId);

            builder.Property(tr => tr.TestResultId)
                .ValueGeneratedOnAdd();

            // UserId
            builder.Property(tr => tr.UserId)
                .IsRequired();

            // TestId
            builder.Property(tr => tr.TestId)
                .IsRequired();

            // Score
            builder.Property(tr => tr.Score)
                .IsRequired()
                .HasPrecision(5, 2);

            // CorrectCount
            builder.Property(tr => tr.CorrectCount)
                .IsRequired();

            // TotalQuestion
            builder.Property(tr => tr.TotalQuestion)
                .IsRequired();

            // SubmittedAt
            builder.Property(tr => tr.SubmittedAt)
                .IsRequired()
                .HasDefaultValueSql("GETDATE()");

            // TestResult N - 1 User
            builder.HasOne(tr => tr.User)
                .WithMany(u => u.TestResults)
                .HasForeignKey(tr => tr.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            // TestResult N - 1 Test
            builder.HasOne(tr => tr.Test)
                .WithMany(t => t.TestResults)
                .HasForeignKey(tr => tr.TestId)
                .OnDelete(DeleteBehavior.Restrict);

            // TestResult 1 - N UserAnswer
            builder.HasMany(tr => tr.UserAnswers)
                .WithOne(ua => ua.TestResult)
                .HasForeignKey(ua => ua.TestResultId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}