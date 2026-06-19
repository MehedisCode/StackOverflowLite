using Microsoft.EntityFrameworkCore;
using StackOverflowLite.Application.Common.Interfaces;
using StackOverflowLite.Domain.Entities;

namespace StackOverflowLite.Infrastructure.Persistence.Repositories;

public class QuestionRepository(ApplicationDbContext db)
    : Repository<Question>(db), IQuestionRepository
{
    public Task<Question?> GetByIdWithTagsAsync(Guid id, CancellationToken cancellationToken = default) =>
        Db.Questions
            .Include(q => q.QuestionTags)
                .ThenInclude(qt => qt.Tag)
            .FirstOrDefaultAsync(q => q.Id == id, cancellationToken);

    public async Task<(IReadOnlyList<Question> Items, int TotalCount)> ListAsync(
        ListQuestionsFilter filter, CancellationToken cancellationToken = default)
    {
        var query = Db.Questions
            .Include(q => q.QuestionTags)
            .ThenInclude(qt => qt.Tag)
            .AsNoTracking();

        if (!string.IsNullOrWhiteSpace(filter.Search))
        {
            var pattern = $"%{filter.Search}%";
            query = query.Where(q => EF.Functions.ILike(q.Title, pattern));
        }

        if (!string.IsNullOrWhiteSpace(filter.TagName))
        {
            var tag = filter.TagName;
            query = query.Where(q => q.QuestionTags.Any(qt => qt.Tag.Name == tag));
        }

        var total = await query.CountAsync(cancellationToken);

        var items = await query
            .OrderByDescending(q => q.CreatedAt)
            .Skip((filter.Page - 1) * filter.PageSize)
            .Take(filter.PageSize)
            .ToListAsync(cancellationToken);

        return (items, total);
    }
}
