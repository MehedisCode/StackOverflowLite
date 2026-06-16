using StackOverflowLite.Domain.Enums;

namespace StackOverflowLite.Domain.Entities;

public class ReputationHistory
{
    public Guid Id { get; init; } = Guid.NewGuid();
    public Guid UserId { get; init; }
    public Guid? SourceUserId { get; init; }
    public int Delta { get; init; }
    public string Reason { get; set; } = string.Empty;
    public VoteTargetType? TargetType { get; init; }
    public Guid? TargetId { get; init; }
    public DateTime CreatedAt { get; init; } = DateTime.UtcNow;
}
