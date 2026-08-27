using HeThongHocNgoaiNguTrucTuyen.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HeThongHocNgoaiNguTrucTuyen.Data.Configurations
{
    public class QuestionConfiguration : IEntityTypeConfiguration<Question>
    {
        public void Configure(EntityTypeBuilder<Question> builder)
        {
            // Table
            builder.ToTable("Questions");

            // Primary Key
            builder.HasKey(q => q.QuestionId);

            builder.Property(q => q.QuestionId)
                .ValueGeneratedOnAdd();

            // TestId
            builder.Property(q => q.TestId)
                .IsRequired();

            // Content
            builder.Property(q => q.Content)
                .IsRequired()
                .HasMaxLength(1000);

            // ImageUrl
            builder.Property(q => q.ImageUrl)
                .IsRequired(false)
                .HasMaxLength(500);

            // PartNumber
            builder.Property(q => q.PartNumber)
                .IsRequired(false);

            // QuestionType
            builder.Property(q => q.QuestionType)
                .IsRequired()
                .HasConversion<int>();

            // QuestionOrder dung de xac dinh lai id cua cau hoi khi QuestionId tang cao
            // (Id = 101 trong test 5 admin dat thanh cau 1 trong test 5 => questionOrder = 1)
            builder.Property(q => q.QuestionOrder)
                .IsRequired();

            // AudioUrl
            builder.Property(q => q.AudioUrl)
                .IsRequired(false)
                .HasMaxLength(500);

            // GroupCode
            builder.Property(q => q.GroupCode)
                .IsRequired(false)
                .HasMaxLength(50);

            // Question N - 1 Test
            builder.HasOne(q => q.Test)
                .WithMany(t => t.Questions)
                .HasForeignKey(q => q.TestId)
                .OnDelete(DeleteBehavior.Cascade);

            // Question 1 - N Answer
            builder.HasMany(q => q.Answers)
                .WithOne(a => a.Question)
                .HasForeignKey(a => a.QuestionId)
                .OnDelete(DeleteBehavior.Cascade);

            // Question 1 - N UserAnswer
            builder.HasMany(q => q.UserAnswers)
                .WithOne(ua => ua.Question)
                .HasForeignKey(ua => ua.QuestionId)
                .OnDelete(DeleteBehavior.Restrict);

            // Mỗi Question trong cùng một Test có QuestionOrder duy nhất
            builder.HasIndex(q => new
            {
                q.TestId,
                q.QuestionOrder
            }).IsUnique();


            // Check QuestionOrder > 0
            builder.ToTable("Questions", table =>
            {
                table.HasCheckConstraint(
                    "CK_Questions_QuestionOrder",
                    "[QuestionOrder] > 0"
                );

                table.HasCheckConstraint(
                    "CK_Questions_PartNumber",
                    "[PartNumber] IS NULL OR [PartNumber] > 0"
                );
            });
        }
    }
}