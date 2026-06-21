using Microsoft.EntityFrameworkCore;
using StackOverflowLite.Application.Common.Interfaces;
using StackOverflowLite.Domain.Entities;
using StackOverflowLite.Domain.Enums;

namespace StackOverflowLite.Infrastructure.Persistence.Repositories;

public class VoteRepository(ApplicationDbContext db)
    : Repository<Vote>(db), IVoteRepository
{
    public Task<Vote?> GetUserVoteForTargetAsync(
        Guid userId, Guid targetId, VoteTargetType targetType,
        CancellationToken cancellationToken = default) =>
        Db.Votes.FirstOrDefaultAsync(
            v => v.UserId == userId && v.TargetId == targetId && v.TargetType == targetType,
            cancellationToken);
}
