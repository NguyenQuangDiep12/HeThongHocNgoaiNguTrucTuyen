using HeThongHocNgoaiNguTrucTuyen.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HeThongHocNgoaiNguTrucTuyen.Data.Configurations
{
    public class AnswerConfiguration : IEntityTypeConfiguration<Answer>
    {
        public void Configure(EntityTypeBuilder<Answer> builder)
        {
            // Table
            builder.ToTable("Answers");

            // Primary Key
            builder.HasKey(a => a.AnswerId);

            builder.Property(a => a.AnswerId)
                .ValueGeneratedOnAdd();

            // QuestionId
            builder.Property(a => a.QuestionId)
                .IsRequired();

            // Content
            builder.Property(a => a.Content)
                .IsRequired()
                .HasMaxLength(500);

            // IsCorrect
            builder.Property(a => a.IsCorrect)
                .IsRequired();

            // Answer N - 1 Question
            builder.HasOne(a => a.Question)
                .WithMany(q => q.Answers)
                .HasForeignKey(a => a.QuestionId)
                .OnDelete(DeleteBehavior.Cascade);

            // Answer 1 - N UserAnswer
            builder.HasMany(a => a.UserAnswers)
                .WithOne(ua => ua.Answer)
                .HasForeignKey(ua => ua.AnswerId)
                .OnDelete(DeleteBehavior.SetNull);
        }
    }
}