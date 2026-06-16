using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using StackOverflowLite.Domain.Entities;
using StackOverflowLite.Infrastructure.Identity;

namespace StackOverflowLite.Infrastructure.Persistence.Configurations;

public class VoteConfiguration : IEntityTypeConfiguration<Vote>
{
    public void Configure(EntityTypeBuilder<Vote> builder)
    {
        builder.ToTable("votes");

        builder.HasKey(v => v.Id);

        builder.Property(v => v.TargetType)
            .HasConversion<int>()
            .IsRequired();

        builder.Property(v => v.VoteType)
            .HasConversion<int>()
            .IsRequired();

        builder.Property(v => v.CreatedAt)
            .IsRequired();

        builder.Property(v => v.UpdatedAt);

        builder.HasOne<ApplicationUser>()
            .WithMany(u => u.Votes)
            .HasForeignKey(v => v.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(v => new { v.UserId, v.TargetId, v.TargetType })
            .IsUnique()
            .HasDatabaseName("IX_Votes_User_Target_Unique");

        builder.HasIndex(v => new { v.TargetId, v.TargetType })
            .HasDatabaseName("IX_Votes_Target");
    }
}
