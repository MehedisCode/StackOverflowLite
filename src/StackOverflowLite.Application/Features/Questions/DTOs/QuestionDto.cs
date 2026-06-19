namespace StackOverflowLite.Application.Features.Questions.DTOs;

public record QuestionDto(
    Guid Id,
    string Title,
    string Body,
    Guid AuthorId,
    int Score,
    long ViewCount,
    int AnswerCount,
    DateTime CreatedAt,
    DateTime? UpdatedAt,
    string[] Tags);
