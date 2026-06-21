using StackOverflowLite.Domain.Enums;

namespace StackOverflowLite.Application.Common.Interfaces;

public interface IVoteService
{
    Task CastVoteAsync(
        Guid userId,
        VoteTargetType targetType,
        Guid targetId,
        VoteType voteType,
        CancellationToken cancellationToken = default);

    Task RemoveVoteAsync(
        Guid userId,
        VoteTargetType targetType,
        Guid targetId,
        CancellationToken cancellationToken = default);
}
