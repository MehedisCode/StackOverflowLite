using MediatR;
using StackOverflowLite.Application.Common.Exceptions;
using StackOverflowLite.Application.Common.Interfaces;
using StackOverflowLite.Domain.Enums;

namespace StackOverflowLite.Application.Features.Votes.Commands.DownvoteQuestion;

public class DownvoteQuestionCommandHandler(
    ICurrentUser currentUser,
    IVoteService voteService) : IRequestHandler<DownvoteQuestionCommand, Unit>
{
    public async Task<Unit> Handle(
        DownvoteQuestionCommand request, CancellationToken cancellationToken)
    {
        if (currentUser.Id is null)
        {
            throw new ForbiddenAccessException("User is not authenticated.");
        }

        await voteService.CastVoteAsync(
            currentUser.Id.Value,
            VoteTargetType.Question,
            request.QuestionId,
            VoteType.Downvote,
            cancellationToken);

        return Unit.Value;
    }
}
