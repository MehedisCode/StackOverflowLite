namespace StackOverflowLite.Application.Features.Questions.DTOs;

public record QuestionListItemDto(
    Guid Id,
    string Title,
    Guid AuthorId,
    int Score,
    int AnswerCount,
    DateTime CreatedAt,
    string[] Tags);
