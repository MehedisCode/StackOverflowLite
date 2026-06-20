namespace StackOverflowLite.Application.Features.Answers.DTOs;

public record AnswerListItemDto(
    Guid Id,
    Guid AuthorId,
    string Body,
    int Score,
    bool IsAccepted,
    DateTime CreatedAt);
