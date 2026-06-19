using MediatR;

namespace StackOverflowLite.Application.Features.Tags.Commands.RemoveTagFromQuestion;

public record RemoveTagFromQuestionCommand(Guid QuestionId, string TagName) : IRequest<Unit>;
