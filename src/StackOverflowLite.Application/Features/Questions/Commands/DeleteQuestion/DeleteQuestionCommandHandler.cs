using MediatR;
using StackOverflowLite.Application.Common.Exceptions;
using StackOverflowLite.Application.Common.Interfaces;

namespace StackOverflowLite.Application.Features.Questions.Commands.DeleteQuestion;

public class DeleteQuestionCommandHandler(
    ICurrentUser currentUser,
    IUnitOfWork unitOfWork,
    ICacheService cacheService) : IRequestHandler<DeleteQuestionCommand, Unit>
{
    public async Task<Unit> Handle(
        DeleteQuestionCommand request, CancellationToken cancellationToken)
    {
        if (currentUser.Id is null)
        {
            throw new ForbiddenAccessException("User is not authenticated.");
        }

        var question = await unitOfWork.Questions.GetByIdAsync(request.Id, cancellationToken)
            ?? throw new NotFoundException($"Question {request.Id} not found.");

        if (question.AuthorId != currentUser.Id.Value)
        {
            throw new ForbiddenAccessException("You can only delete your own questions.");
        }

        unitOfWork.Questions.Delete(question);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        await cacheService.RemoveAsync($"question:{request.Id}", cancellationToken);
        await cacheService.RemoveAsync($"user:profile:{question.AuthorId}", cancellationToken);

        return Unit.Value;
    }
}
