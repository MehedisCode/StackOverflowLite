using StackOverflowLite.Domain.Enums;

namespace StackOverflowLite.Domain.Entities;

public class Vote
{
    private VoteType _voteType;

    public Guid Id { get; init; } = Guid.NewGuid();
    public Guid UserId { get; init; }
    public Guid TargetId { get; init; }
    public VoteTargetType TargetType { get; init; }
    public VoteType VoteType
    {
        get => _voteType;
        init => _voteType = value;
    }
    public DateTime CreatedAt { get; init; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; private set; }

    public void ChangeVoteType(VoteType newType) => _voteType = newType;

    public void MarkUpdated() => UpdatedAt = DateTime.UtcNow;
}
