using HeThongHocNgoaiNguTrucTuyen.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HeThongHocNgoaiNguTrucTuyen.Data.Configurations
{
    public class LanguageConfiguration : IEntityTypeConfiguration<Language>
    {
        public void Configure(EntityTypeBuilder<Language> builder)
        {
            // Table
            builder.ToTable("Languages");

            // Primary Key
            builder.HasKey(l => l.LanguageId);

            builder.Property(l => l.LanguageId)
                .ValueGeneratedOnAdd();

            // Name
            builder.Property(l => l.Name)
                .IsRequired()
                .HasMaxLength(100);

            // Code
            builder.Property(l => l.Code)
                .IsRequired()
                .HasMaxLength(20);

            // Code phải duy nhất
            builder.HasIndex(l => l.Code)
                .IsUnique();

            // Description
            builder.Property(l => l.Description)
                .IsRequired(false)
                .HasMaxLength(500);

            // Language 1 - N Topic
            builder.HasMany(l => l.Topics)
                .WithOne(t => t.Language)
                .HasForeignKey(t => t.LanguageId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}