using Microsoft.EntityFrameworkCore;
using StackOverflowLite.Application.Common.Interfaces;
using StackOverflowLite.Domain.Entities;

namespace StackOverflowLite.Infrastructure.Persistence.Repositories;

public class TagRepository(ApplicationDbContext db)
    : Repository<Tag>(db), ITagRepository
{
    public Task<Tag?> GetByNameAsync(string name, CancellationToken cancellationToken = default)
    {
        var normalized = name.ToLowerInvariant();
        return Db.Tags.FirstOrDefaultAsync(t => t.Name == normalized, cancellationToken);
    }

    public async Task<IReadOnlyList<Tag>> GetByNamesAsync(
        IEnumerable<string> names, CancellationToken cancellationToken = default)
    {
        var normalized = names.Select(n => n.ToLowerInvariant()).ToList();
        return await Db.Tags
            .Where(t => normalized.Contains(t.Name))
            .ToListAsync(cancellationToken);
    }

    public async Task<(IReadOnlyList<Tag> Items, int TotalCount)> ListAsync(
        ListTagsFilter filter, CancellationToken cancellationToken = default)
    {
        var query = Db.Tags.AsNoTracking();

        if (!string.IsNullOrWhiteSpace(filter.Search))
        {
            var pattern = $"%{filter.Search.ToLowerInvariant()}%";
            query = query.Where(t => EF.Functions.ILike(t.Name, pattern));
        }

        var total = await query.CountAsync(cancellationToken);

        var items = await query
            .OrderBy(t => t.Name)
            .Skip((filter.Page - 1) * filter.PageSize)
            .Take(filter.PageSize)
            .ToListAsync(cancellationToken);

        return (items, total);
    }
}
