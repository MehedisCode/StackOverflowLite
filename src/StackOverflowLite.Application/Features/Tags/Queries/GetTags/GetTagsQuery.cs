using MediatR;
using StackOverflowLite.Application.Common.Models;
using StackOverflowLite.Application.Features.Tags.DTOs;

namespace StackOverflowLite.Application.Features.Tags.Queries.GetTags;

public record GetTagsQuery(
    int Page = 1,
    int PageSize = 20,
    string? Search = null) : IRequest<PagedResult<TagListItemDto>>;
