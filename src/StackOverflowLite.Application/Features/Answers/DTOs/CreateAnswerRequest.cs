namespace StackOverflowLite.Application.Features.Answers.DTOs;

public record CreateAnswerRequest(Guid QuestionId, string Body);
