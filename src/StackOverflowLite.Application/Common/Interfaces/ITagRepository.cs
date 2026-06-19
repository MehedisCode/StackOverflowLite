using StackOverflowLite.Domain.Entities;

namespace StackOverflowLite.Application.Common.Interfaces;

public record ListTagsFilter(string? Search, int Page, int PageSize);

public interface ITagRepository : IRepository<Tag>
{
    Task<Tag?> GetByNameAsync(string name, CancellationToken cancellationToken = default);

    Task<IReadOnlyList<Tag>> GetByNamesAsync(
        IEnumerable<string> names,
        CancellationToken cancellationToken = default);

    Task<(IReadOnlyList<Tag> Items, int TotalCount)> ListAsync(
        ListTagsFilter filter,
        CancellationToken cancellationToken = default);
}
