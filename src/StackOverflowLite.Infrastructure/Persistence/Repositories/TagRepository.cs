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
}
