using MediatR;

namespace StackOverflowLite.Application.Features.Votes.Commands.UpvoteAnswer;

public record UpvoteAnswerCommand(Guid AnswerId) : IRequest<Unit>;
