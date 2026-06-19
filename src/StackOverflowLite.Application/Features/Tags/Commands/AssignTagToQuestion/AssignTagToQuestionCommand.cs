using MediatR;

namespace StackOverflowLite.Application.Features.Tags.Commands.AssignTagToQuestion;

public record AssignTagToQuestionCommand(Guid QuestionId, string TagName) : IRequest<Unit>;
