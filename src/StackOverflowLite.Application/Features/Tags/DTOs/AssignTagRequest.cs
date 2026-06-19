namespace StackOverflowLite.Application.Features.Tags.DTOs;

public record AssignTagRequest(Guid QuestionId, string TagName);
