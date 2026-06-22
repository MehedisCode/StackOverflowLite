using Microsoft.EntityFrameworkCore;
using StackOverflowLite.Application.Common.Interfaces;
using StackOverflowLite.Domain.Entities;

namespace StackOverflowLite.Infrastructure.Persistence.Repositories;

public class AnswerRepository(ApplicationDbContext db)
    : Repository<Answer>(db), IAnswerRepository
{
    public Task<Answer?> GetByIdWithQuestionAsync(
        Guid id, CancellationToken cancellationToken = default) =>
        Db.Answers
            .Include(a => a.Question)
            .FirstOrDefaultAsync(a => a.Id == id, cancellationToken);

    public async Task<(IReadOnlyList<Answer> Items, int TotalCount)> ListAsync(
        ListAnswersFilter filter, CancellationToken cancellationToken = default)
    {
        var query = Db.Answers
            .Where(a => a.QuestionId == filter.QuestionId)
            .AsNoTracking();

        var total = await query.CountAsync(cancellationToken);

        // Order: accepted first, then by score, then by recent
        var items = await query
            .OrderByDescending(a => a.IsAccepted)
            .ThenByDescending(a => a.UpvoteCount)
            .ThenByDescending(a => a.CreatedAt)
            .Skip((filter.Page - 1) * filter.PageSize)
            .Take(filter.PageSize)
            .ToListAsync(cancellationToken);

        return (items, total);
    }

    public async Task<(int Upvotes, int Downvotes, int Accepted)> GetUserAnswerVoteStatsAsync(
        Guid userId, CancellationToken cancellationToken = default)
    {
        var query = Db.Answers.Where(a => a.AuthorId == userId);

        var upvotes = await query.SumAsync(a => a.UpvoteCount, cancellationToken);
        var downvotes = await query.SumAsync(a => a.DownvoteCount, cancellationToken);
        var accepted = await query.CountAsync(a => a.IsAccepted, cancellationToken);

        return (upvotes, downvotes, accepted);
    }
}
