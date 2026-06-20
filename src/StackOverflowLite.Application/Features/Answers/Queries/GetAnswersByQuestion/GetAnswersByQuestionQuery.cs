using MediatR;
using StackOverflowLite.Application.Common.Models;
using StackOverflowLite.Application.Features.Answers.DTOs;

namespace StackOverflowLite.Application.Features.Answers.Queries.GetAnswersByQuestion;

public record GetAnswersByQuestionQuery(
    Guid QuestionId,
    int Page = 1,
    int PageSize = 20) : IRequest<PagedResult<AnswerListItemDto>>;
