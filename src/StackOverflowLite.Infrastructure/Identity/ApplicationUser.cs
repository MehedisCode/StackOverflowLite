using Microsoft.AspNetCore.Identity;
using StackOverflowLite.Domain.Entities;

namespace StackOverflowLite.Infrastructure.Identity;

public class ApplicationUser : IdentityUser<Guid>
{
    public string DisplayName { get; set; } = string.Empty;
    public string? Bio { get; set; }
    public string? AvatarUrl { get; set; }
    public int Reputation { get; private set; } = 1;
    public DateTime CreatedAt { get; init; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; private set; }

    public ICollection<Question> Questions { get; private set; } = [];
    public ICollection<Answer> Answers { get; private set; } = [];
    public ICollection<Vote> Votes { get; private set; } = [];
    public ICollection<ReputationHistory> ReputationHistory { get; private set; } = [];
}
