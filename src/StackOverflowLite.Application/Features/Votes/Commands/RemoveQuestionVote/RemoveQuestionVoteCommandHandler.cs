using MediatR;
using StackOverflowLite.Application.Common.Exceptions;
using StackOverflowLite.Application.Common.Interfaces;
using StackOverflowLite.Domain.Enums;

namespace StackOverflowLite.Application.Features.Votes.Commands.RemoveQuestionVote;

public class RemoveQuestionVoteCommandHandler(
    ICurrentUser currentUser,
    IVoteService voteService) : IRequestHandler<RemoveQuestionVoteCommand, Unit>
{
    public async Task<Unit> Handle(
        RemoveQuestionVoteCommand request, CancellationToken cancellationToken)
    {
        if (currentUser.Id is null)
        {
            throw new ForbiddenAccessException("User is not authenticated.");
        }

        await voteService.RemoveVoteAsync(
            currentUser.Id.Value,
            VoteTargetType.Question,
            request.QuestionId,
            cancellationToken);

        return Unit.Value;
    }
}
