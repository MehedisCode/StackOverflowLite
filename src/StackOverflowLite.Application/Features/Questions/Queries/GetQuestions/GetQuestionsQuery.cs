using MediatR;
using StackOverflowLite.Application.Common.Models;
using StackOverflowLite.Application.Features.Questions.DTOs;

namespace StackOverflowLite.Application.Features.Questions.Queries.GetQuestions;

public record GetQuestionsQuery(
    int Page = 1,
    int PageSize = 20,
    string? Tag = null,
    string? Search = null) : IRequest<PagedResult<QuestionListItemDto>>;
