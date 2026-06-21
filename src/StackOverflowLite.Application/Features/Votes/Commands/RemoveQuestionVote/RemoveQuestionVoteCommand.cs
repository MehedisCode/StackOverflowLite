using MediatR;

namespace StackOverflowLite.Application.Features.Votes.Commands.RemoveQuestionVote;

public record RemoveQuestionVoteCommand(Guid QuestionId) : IRequest<Unit>;
