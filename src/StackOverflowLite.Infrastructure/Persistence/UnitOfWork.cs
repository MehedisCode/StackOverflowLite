using StackOverflowLite.Application.Common.Interfaces;

namespace StackOverflowLite.Infrastructure.Persistence;

public class UnitOfWork(
    ApplicationDbContext db,
    IQuestionRepository questions,
    ITagRepository tags,
    IAnswerRepository answers) : IUnitOfWork
{
    public IQuestionRepository Questions => questions;
    public ITagRepository Tags => tags;
    public IAnswerRepository Answers => answers;

    public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default) =>
        db.SaveChangesAsync(cancellationToken);
}
