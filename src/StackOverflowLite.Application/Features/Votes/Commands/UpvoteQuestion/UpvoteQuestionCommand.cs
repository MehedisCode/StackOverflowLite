using MediatR;

namespace StackOverflowLite.Application.Features.Votes.Commands.UpvoteQuestion;

public record UpvoteQuestionCommand(Guid QuestionId) : IRequest<Unit>;
