using MediatR;
using StackOverflowLite.Application.Common.Exceptions;
using StackOverflowLite.Application.Common.Interfaces;
using StackOverflowLite.Domain.Enums;

namespace StackOverflowLite.Application.Features.Votes.Commands.UpvoteAnswer;

public class UpvoteAnswerCommandHandler(
    ICurrentUser currentUser,
    IVoteService voteService) : IRequestHandler<UpvoteAnswerCommand, Unit>
{
    public async Task<Unit> Handle(
        UpvoteAnswerCommand request, CancellationToken cancellationToken)
    {
        if (currentUser.Id is null)
        {
            throw new ForbiddenAccessException("User is not authenticated.");
        }

        await voteService.CastVoteAsync(
            currentUser.Id.Value,
            VoteTargetType.Answer,
            request.AnswerId,
            VoteType.Upvote,
            cancellationToken);

        return Unit.Value;
    }
}
