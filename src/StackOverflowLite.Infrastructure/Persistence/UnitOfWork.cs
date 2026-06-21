using StackOverflowLite.Application.Common.Interfaces;

namespace StackOverflowLite.Infrastructure.Persistence;

public class UnitOfWork(
    ApplicationDbContext db,
    IQuestionRepository questions,
    ITagRepository tags,
    IAnswerRepository answers,
    IVoteRepository votes) : IUnitOfWork
{
    public IQuestionRepository Questions => questions;
    public ITagRepository Tags => tags;
    public IAnswerRepository Answers => answers;
    public IVoteRepository Votes => votes;

    public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default) =>
        db.SaveChangesAsync(cancellationToken);
}
