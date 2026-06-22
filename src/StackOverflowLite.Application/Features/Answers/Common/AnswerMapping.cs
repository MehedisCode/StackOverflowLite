using StackOverflowLite.Application.Features.Answers.DTOs;
using StackOverflowLite.Domain.Entities;

namespace StackOverflowLite.Application.Features.Answers.Common;

internal static class AnswerMapping
{
    public static AnswerDto ToDto(Answer a) =>
        new(a.Id, a.QuestionId, a.AuthorId, a.Body, a.UpvoteCount, a.DownvoteCount, a.IsAccepted, a.CreatedAt, a.UpdatedAt);

    public static AnswerListItemDto ToListItemDto(Answer a) =>
        new(a.Id, a.AuthorId, a.Body, a.UpvoteCount, a.DownvoteCount, a.IsAccepted, a.CreatedAt);
}
