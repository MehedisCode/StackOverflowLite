using MediatR;
using StackOverflowLite.Application.Common.Exceptions;
using StackOverflowLite.Application.Common.Interfaces;
using StackOverflowLite.Domain.Enums;

namespace StackOverflowLite.Application.Features.Votes.Commands.RemoveAnswerVote;

public class RemoveAnswerVoteCommandHandler(
    ICurrentUser currentUser,
    IVoteService voteService) : IRequestHandler<RemoveAnswerVoteCommand, Unit>
{
    public async Task<Unit> Handle(
        RemoveAnswerVoteCommand request, CancellationToken cancellationToken)
    {
        if (currentUser.Id is null)
        {
            throw new ForbiddenAccessException("User is not authenticated.");
        }

        await voteService.RemoveVoteAsync(
            currentUser.Id.Value,
            VoteTargetType.Answer,
            request.AnswerId,
            cancellationToken);

        return Unit.Value;
    }
}
