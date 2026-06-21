namespace StackOverflowLite.Application.Common.Interfaces;

public interface IUnitOfWork
{
    IQuestionRepository Questions { get; }
    ITagRepository Tags { get; }
    IAnswerRepository Answers { get; }
    IVoteRepository Votes { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
