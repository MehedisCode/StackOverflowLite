using MediatR;

namespace StackOverflowLite.Application.Features.Votes.Commands.DownvoteAnswer;

public record DownvoteAnswerCommand(Guid AnswerId) : IRequest<Unit>;
