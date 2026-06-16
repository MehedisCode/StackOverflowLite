using StackOverflowLite.Domain.Enums;

namespace StackOverflowLite.Domain.Entities;

public class Vote
{
    public Guid Id { get; init; } = Guid.NewGuid();
    public Guid UserId { get; init; }
    public Guid TargetId { get; init; }
    public VoteTargetType TargetType { get; init; }
    public VoteType VoteType { get; init; }
    public DateTime CreatedAt { get; init; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; private set; }
}
