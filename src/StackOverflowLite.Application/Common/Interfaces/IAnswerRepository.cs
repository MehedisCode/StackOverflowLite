using StackOverflowLite.Domain.Entities;

namespace StackOverflowLite.Application.Common.Interfaces;

public record ListAnswersFilter(Guid QuestionId, int Page, int PageSize);

public interface IAnswerRepository : IRepository<Answer>
{
    Task<Answer?> GetByIdWithQuestionAsync(
        Guid id,
        CancellationToken cancellationToken = default);

    Task<(IReadOnlyList<Answer> Items, int TotalCount)> ListAsync(
        ListAnswersFilter filter,
        CancellationToken cancellationToken = default);

    Task<(int Upvotes, int Downvotes, int Accepted)> GetUserAnswerVoteStatsAsync(
        Guid userId,
        CancellationToken cancellationToken = default);
}
