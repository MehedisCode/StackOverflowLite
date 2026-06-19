using MediatR;
using StackOverflowLite.Application.Common.Models;
using StackOverflowLite.Application.Features.Questions.DTOs;

namespace StackOverflowLite.Application.Features.Tags.Queries.GetQuestionsByTag;

public record GetQuestionsByTagQuery(
    string TagName,
    int Page = 1,
    int PageSize = 20) : IRequest<PagedResult<QuestionListItemDto>>;
