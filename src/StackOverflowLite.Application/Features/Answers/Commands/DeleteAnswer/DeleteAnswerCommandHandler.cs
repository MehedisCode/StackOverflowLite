using MediatR;
using StackOverflowLite.Application.Common.Exceptions;
using StackOverflowLite.Application.Common.Interfaces;

namespace StackOverflowLite.Application.Features.Answers.Commands.DeleteAnswer;

public class DeleteAnswerCommandHandler(
    ICurrentUser currentUser,
    IUnitOfWork unitOfWork) : IRequestHandler<DeleteAnswerCommand, Unit>
{
    public async Task<Unit> Handle(
        DeleteAnswerCommand request, CancellationToken cancellationToken)
    {
        if (currentUser.Id is null)
        {
            throw new ForbiddenAccessException("User is not authenticated.");
        }

        var answer = await unitOfWork.Answers.GetByIdWithQuestionAsync(request.Id, cancellationToken)
            ?? throw new NotFoundException($"Answer {request.Id} not found.");

        if (answer.AuthorId != currentUser.Id.Value)
        {
            throw new ForbiddenAccessException("You can only delete your own answers.");
        }

        if (answer.Question.AcceptedAnswerId == answer.Id)
        {
            answer.Question.ClearAcceptedAnswer();
        }

        answer.Question.DecrementAnswerCount();

        unitOfWork.Questions.Update(answer.Question);
        unitOfWork.Answers.Delete(answer);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}
