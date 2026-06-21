using MediatR;

namespace StackOverflowLite.Application.Features.Votes.Commands.RemoveAnswerVote;

public record RemoveAnswerVoteCommand(Guid AnswerId) : IRequest<Unit>;
