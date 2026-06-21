using StackOverflowLite.Domain.Entities;
using StackOverflowLite.Domain.Enums;

namespace StackOverflowLite.Application.Common.Interfaces;

public interface IVoteRepository : IRepository<Vote>
{
    Task<Vote?> GetUserVoteForTargetAsync(
        Guid userId,
        Guid targetId,
        VoteTargetType targetType,
        CancellationToken cancellationToken = default);
}
