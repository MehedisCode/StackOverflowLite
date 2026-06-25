namespace StackOverflowLite.Application.Features.Questions.DTOs;

public record QuestionDto(
    Guid Id,
    string Title,
    string Body,
    Guid AuthorId,
    int UpvoteCount,
    int DownvoteCount,
    int AnswerCount,
    int ViewCount,
    DateTime CreatedAt,
    DateTime? UpdatedAt,
    string[] Tags);
