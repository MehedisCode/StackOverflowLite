using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using StackOverflowLite.Domain.Entities;

namespace StackOverflowLite.Infrastructure.Persistence.Configurations;

public class TagConfiguration : IEntityTypeConfiguration<Tag>
{
    public void Configure(EntityTypeBuilder<Tag> builder)
    {
        builder.ToTable("tags");

        builder.HasKey(t => t.Id);

        builder.Property(t => t.Name)
            .IsRequired()
            .HasMaxLength(25);

        builder.Property(t => t.Description)
            .HasMaxLength(500);

        builder.Property(t => t.UsageCount)
            .HasDefaultValue(0);

        builder.Property(t => t.CreatedAt)
            .IsRequired();

        builder.Property(t => t.UpdatedAt);

        builder.HasMany(t => t.QuestionTags)
            .WithOne(qt => qt.Tag)
            .HasForeignKey(qt => qt.TagId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex(t => t.Name)
            .IsUnique()
            .HasDatabaseName("IX_Tags_Name_Unique");

        builder.HasIndex(t => t.UsageCount)
            .IsDescending()
            .HasDatabaseName("IX_Tags_UsageCount");
    }
}
