namespace StackOverflowLite.Application.Features.Answers.DTOs;

public record AnswerListItemDto(
    Guid Id,
    Guid AuthorId,
    string Body,
    int UpvoteCount,
    int DownvoteCount,
    bool IsAccepted,
    DateTime CreatedAt);
