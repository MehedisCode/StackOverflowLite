using MediatR;
using StackOverflowLite.Application.Common.Exceptions;
using StackOverflowLite.Application.Common.Interfaces;

namespace StackOverflowLite.Application.Features.Answers.Commands.UnacceptAnswer;

public class UnacceptAnswerCommandHandler(
    ICurrentUser currentUser,
    IUnitOfWork unitOfWork) : IRequestHandler<UnacceptAnswerCommand, Unit>
{
    public async Task<Unit> Handle(
        UnacceptAnswerCommand request, CancellationToken cancellationToken)
    {
        if (currentUser.Id is null)
        {
            throw new ForbiddenAccessException("User is not authenticated.");
        }

        var answer = await unitOfWork.Answers.GetByIdWithQuestionAsync(request.AnswerId, cancellationToken)
            ?? throw new NotFoundException($"Answer {request.AnswerId} not found.");

        if (answer.Question.AuthorId != currentUser.Id.Value)
        {
            throw new ForbiddenAccessException("Only the question owner can remove the accepted answer.");
        }

        // not currently the accepted answer
        if (answer.Question.AcceptedAnswerId != answer.Id)
        {
            return Unit.Value;
        }

        answer.MarkUnaccepted();
        answer.Question.ClearAcceptedAnswer();

        unitOfWork.Questions.Update(answer.Question);
        unitOfWork.Answers.Update(answer);

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}
