namespace StackOverflowLite.Application.Features.Answers.DTOs;

public record AnswerDto(
    Guid Id,
    Guid QuestionId,
    Guid AuthorId,
    string Body,
    int UpvoteCount,
    int DownvoteCount,
    bool IsAccepted,
    DateTime CreatedAt,
    DateTime? UpdatedAt);
