using StackOverflowLite.Application.Features.Questions.DTOs;
using StackOverflowLite.Domain.Entities;

namespace StackOverflowLite.Application.Features.Questions.Common;

internal static class QuestionMapping
{
    public static string[] NormalizeTags(string[]? raw)
    {
        if (raw is null || raw.Length == 0)
        {
            return [];
        }

        return raw
            .Where(t => !string.IsNullOrWhiteSpace(t))
            .Select(t => t.Trim().ToLowerInvariant())
            .Distinct()
            .ToArray();
    }

    public static string[] GetTagNames(Question question) =>
        question.QuestionTags
            .Select(qt => qt.Tag.Name)
            .ToArray();

    public static QuestionDto ToDto(Question question) =>
        new(
            question.Id,
            question.Title,
            question.Body,
            question.AuthorId,
            question.UpvoteCount,
            question.DownvoteCount,
            question.AnswerCount,
            question.ViewCount,
            question.CreatedAt,
            question.UpdatedAt,
            GetTagNames(question));

    public static QuestionListItemDto ToListItemDto(Question question) =>
        new(
            question.Id,
            question.Title,
            question.AuthorId,
            question.UpvoteCount,
            question.DownvoteCount,
            question.AnswerCount,
            question.ViewCount,
            question.CreatedAt,
            GetTagNames(question));
}
