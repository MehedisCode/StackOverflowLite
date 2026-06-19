using MediatR;
using StackOverflowLite.Application.Common.Interfaces;
using StackOverflowLite.Application.Common.Models;
using StackOverflowLite.Application.Features.Tags.Common;
using StackOverflowLite.Application.Features.Tags.DTOs;

namespace StackOverflowLite.Application.Features.Tags.Queries.GetTags;

public class GetTagsQueryHandler(IUnitOfWork unitOfWork)
    : IRequestHandler<GetTagsQuery, PagedResult<TagListItemDto>>
{
    public async Task<PagedResult<TagListItemDto>> Handle(
        GetTagsQuery request, CancellationToken cancellationToken)
    {
        var search = string.IsNullOrWhiteSpace(request.Search)
            ? null
            : request.Search.Trim();

        var (items, total) = await unitOfWork.Tags.ListAsync(
            new ListTagsFilter(search, request.Page, request.PageSize),
            cancellationToken);

        var mapped = items.Select(TagMapping.ToListItemDto).ToList();

        return new PagedResult<TagListItemDto>(
            mapped, request.Page, request.PageSize, total);
    }
}
