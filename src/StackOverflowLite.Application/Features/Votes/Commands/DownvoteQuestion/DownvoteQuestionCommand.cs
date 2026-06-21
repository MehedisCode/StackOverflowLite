using MediatR;

namespace StackOverflowLite.Application.Features.Votes.Commands.DownvoteQuestion;

public record DownvoteQuestionCommand(Guid QuestionId) : IRequest<Unit>;
