using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using StackOverflowLite.Domain.Entities;
using StackOverflowLite.Infrastructure.Identity;

namespace StackOverflowLite.Infrastructure.Persistence.Configurations;

public class ReputationHistoryConfiguration : IEntityTypeConfiguration<ReputationHistory>
{
    public void Configure(EntityTypeBuilder<ReputationHistory> builder)
    {
        builder.ToTable("reputation_history");

        builder.HasKey(rh => rh.Id);

        builder.Property(rh => rh.Delta)
            .IsRequired();

        builder.Property(rh => rh.Reason)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(rh => rh.TargetType)
            .HasConversion<int?>();

        builder.Property(rh => rh.CreatedAt)
            .IsRequired();

        builder.HasOne<ApplicationUser>()
            .WithMany(u => u.ReputationHistory)
            .HasForeignKey(rh => rh.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne<ApplicationUser>()
            .WithMany()
            .HasForeignKey(rh => rh.SourceUserId)
            .OnDelete(DeleteBehavior.Restrict)
            .IsRequired(false);

        builder.HasIndex(rh => new { rh.UserId, rh.CreatedAt })
            .IsDescending(false, true)
            .HasDatabaseName("IX_ReputationHistory_UserId_CreatedAt");

        builder.HasIndex(rh => new { rh.TargetType, rh.TargetId })
            .HasDatabaseName("IX_ReputationHistory_Target");
    }
}
