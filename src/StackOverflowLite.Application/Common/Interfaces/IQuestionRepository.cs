using StackOverflowLite.Domain.Entities;

namespace StackOverflowLite.Application.Common.Interfaces;

public record ListQuestionsFilter(string? Search, string? TagName, int Page, int PageSize);

public interface IQuestionRepository : IRepository<Question>
{
    Task<Question?> GetByIdWithTagsAsync(Guid id, CancellationToken cancellationToken = default);

    Task<(IReadOnlyList<Question> Items, int TotalCount)> ListAsync(
        ListQuestionsFilter filter,
        CancellationToken cancellationToken = default);

    Task<(int Upvotes, int Downvotes)> GetUserQuestionVoteStatsAsync(
        Guid userId,
        CancellationToken cancellationToken = default);
}
