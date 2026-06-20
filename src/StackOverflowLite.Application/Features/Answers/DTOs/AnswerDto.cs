namespace StackOverflowLite.Application.Features.Answers.DTOs;

public record AnswerDto(
    Guid Id,
    Guid QuestionId,
    Guid AuthorId,
    string Body,
    int Score,
    bool IsAccepted,
    DateTime CreatedAt,
    DateTime? UpdatedAt);
