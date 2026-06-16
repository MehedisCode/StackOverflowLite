using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using StackOverflowLite.Domain.Entities;
using StackOverflowLite.Infrastructure.Identity;

namespace StackOverflowLite.Infrastructure.Persistence.Configurations;

public class QuestionConfiguration : IEntityTypeConfiguration<Question>
{
    public void Configure(EntityTypeBuilder<Question> builder)
    {
        builder.ToTable("questions");

        builder.HasKey(q => q.Id);

        builder.Property(q => q.Title)
            .IsRequired()
            .HasMaxLength(150);

        builder.Property(q => q.Body)
            .IsRequired()
            .HasColumnType("text");

        builder.Property(q => q.Score)
            .HasDefaultValue(0);

        builder.Property(q => q.ViewCount)
            .HasDefaultValue(0L);

        builder.Property(q => q.AnswerCount)
            .HasDefaultValue(0);

        builder.Property(q => q.CreatedAt)
            .IsRequired();

        builder.Property(q => q.UpdatedAt);

        builder.HasOne<ApplicationUser>()
            .WithMany(u => u.Questions)
            .HasForeignKey(q => q.AuthorId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(q => q.Answers)
            .WithOne(a => a.Question)
            .HasForeignKey(a => a.QuestionId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(q => q.QuestionTags)
            .WithOne(qt => qt.Question)
            .HasForeignKey(qt => qt.QuestionId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(q => q.AcceptedAnswer)
            .WithMany()
            .HasForeignKey(q => q.AcceptedAnswerId)
            .OnDelete(DeleteBehavior.Restrict)
            .IsRequired(false);

        builder.HasIndex(q => q.AuthorId)
            .HasDatabaseName("IX_Questions_AuthorId");

        builder.HasIndex(q => q.Score)
            .IsDescending()
            .HasDatabaseName("IX_Questions_Score");

        builder.HasIndex(q => q.CreatedAt)
            .IsDescending()
            .HasDatabaseName("IX_Questions_CreatedAt");
    }
}
