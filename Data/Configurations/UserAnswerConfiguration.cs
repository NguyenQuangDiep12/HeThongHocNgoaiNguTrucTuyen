using HeThongHocNgoaiNguTrucTuyen.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HeThongHocNgoaiNguTrucTuyen.Data.Configurations
{
    public class UserAnswerConfiguration : IEntityTypeConfiguration<UserAnswer>
    {
        public void Configure(EntityTypeBuilder<UserAnswer> builder)
        {
            // Table
            builder.ToTable("UserAnswers");

            // Primary Key
            builder.HasKey(ua => ua.UserAnswerId);

            builder.Property(ua => ua.UserAnswerId)
                .ValueGeneratedOnAdd();

            // TestResultId
            builder.Property(ua => ua.TestResultId)
                .IsRequired();

            // QuestionId
            builder.Property(ua => ua.QuestionId)
                .IsRequired();

            // AnswerId
            // Nullable -> người học có thể không chọn đáp án
            builder.Property(ua => ua.AnswerId)
                .IsRequired(false);

            // IsCorrect
            builder.Property(ua => ua.IsCorrect)
                .IsRequired();

            // UserAnswer N - 1 TestResult
            builder.HasOne(ua => ua.TestResult)
                .WithMany(tr => tr.UserAnswers)
                .HasForeignKey(ua => ua.TestResultId)
                .OnDelete(DeleteBehavior.Cascade);

            // UserAnswer N - 1 Question
            builder.HasOne(ua => ua.Question)
                .WithMany(q => q.UserAnswers)
                .HasForeignKey(ua => ua.QuestionId)
                .OnDelete(DeleteBehavior.Restrict);

            // UserAnswer N - 1 Answer
            // AnswerId nullable -> optional relationship
            builder.HasOne(ua => ua.Answer)
                .WithMany(a => a.UserAnswers)
                .HasForeignKey(ua => ua.AnswerId)
                .OnDelete(DeleteBehavior.SetNull);
        }
    }
}