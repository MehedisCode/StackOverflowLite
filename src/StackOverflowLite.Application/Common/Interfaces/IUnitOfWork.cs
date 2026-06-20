namespace StackOverflowLite.Application.Common.Interfaces;

public interface IUnitOfWork
{
    IQuestionRepository Questions { get; }
    ITagRepository Tags { get; }
    IAnswerRepository Answers { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
