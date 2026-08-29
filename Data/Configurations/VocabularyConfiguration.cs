using HeThongHocNgoaiNguTrucTuyen.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HeThongHocNgoaiNguTrucTuyen.Data.Configurations
{
    public class VocabularyConfiguration : IEntityTypeConfiguration<Vocabulary>
    {
        public void Configure(EntityTypeBuilder<Vocabulary> builder)
        {
            // Table
            builder.ToTable("Vocabularies");

            // Primary Key
            builder.HasKey(v => v.VocabularyId);

            builder.Property(v => v.VocabularyId)
                .ValueGeneratedOnAdd();

            // LessonId
            builder.Property(v => v.LessonId)
                .IsRequired();

            // Word
            builder.Property(v => v.Word)
                .IsRequired()
                .HasMaxLength(100);

            // Meaning
            builder.Property(v => v.Meaning)
                .IsRequired()
                .HasMaxLength(255);
            // Phonenic
            builder.Property(v => v.Phoenic)
                .IsRequired()
                .HasColumnType("nvarchar")
                .HasMaxLength(30);
            // Example
            builder.Property(v => v.Example)
                .IsRequired(false)
                .HasMaxLength(500);

            // Vocabulary N - 1 Lesson
            builder.HasOne(v => v.Lesson)
                .WithMany(l => l.Vocabularies)
                .HasForeignKey(v => v.LessonId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}