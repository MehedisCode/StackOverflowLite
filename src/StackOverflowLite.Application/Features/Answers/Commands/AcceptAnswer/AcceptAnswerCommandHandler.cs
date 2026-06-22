using MediatR;
using Microsoft.Extensions.Logging;
using StackOverflowLite.Application.Common.Exceptions;
using StackOverflowLite.Application.Common.Interfaces;

namespace StackOverflowLite.Application.Features.Answers.Commands.AcceptAnswer;

public class AcceptAnswerCommandHandler(
    ICurrentUser currentUser,
    IUnitOfWork unitOfWork,
    ILogger<AcceptAnswerCommandHandler> logger
    ) : IRequestHandler<AcceptAnswerCommand, Unit>
{
    public async Task<Unit> Handle(
        AcceptAnswerCommand request, CancellationToken cancellationToken)
    {
        if (currentUser.Id is null)
        {
            throw new ForbiddenAccessException("User is not authenticated.");
        }

        var answer = await unitOfWork.Answers.GetByIdWithQuestionAsync(request.AnswerId, cancellationToken)
            ?? throw new NotFoundException($"Answer {request.AnswerId} not found.");

        if (answer.Question.AuthorId != currentUser.Id.Value)
        {
            throw new ForbiddenAccessException("Only the question owner can accept an answer.");
        }

        // author can't accept own answer
        if (answer.AuthorId == currentUser.Id.Value)
        {
            throw new CannotAcceptOwnAnswerException();
        }

        // already accepted
        if (answer.Question.AcceptedAnswerId == answer.Id)
        {
            return Unit.Value;
        }

        // unaccept the previously accepted answer (if any)
        if (answer.Question.AcceptedAnswerId is Guid oldAcceptedId && oldAcceptedId != answer.Id)
        {
            var oldAccepted = await unitOfWork.Answers.GetByIdAsync(oldAcceptedId, cancellationToken);
            if (oldAccepted is not null)
            {
                oldAccepted.MarkUnaccepted();
                unitOfWork.Answers.Update(oldAccepted);
            }
            else
            {
                logger.LogWarning("Question {QuestionId} referenced missing AcceptedAnswerId {OldAcceptedId}",
                answer.Question.Id, oldAcceptedId);
            }
        }

        answer.MarkAccepted();
        answer.Question.SetAcceptedAnswer(answer.Id);

        unitOfWork.Questions.Update(answer.Question);
        unitOfWork.Answers.Update(answer);

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}
