using MediatR;
using StackOverflowLite.Application.Common.Interfaces;
using StackOverflowLite.Application.Common.Models;
using StackOverflowLite.Application.Features.Questions.Common;
using StackOverflowLite.Application.Features.Questions.DTOs;

namespace StackOverflowLite.Application.Features.Questions.Queries.GetQuestions;

public class GetQuestionsQueryHandler(
    IUnitOfWork unitOfWork,
    ICacheService cacheService) : IRequestHandler<GetQuestionsQuery, PagedResult<QuestionListItemDto>>
{
    public async Task<PagedResult<QuestionListItemDto>> Handle(
        GetQuestionsQuery request, CancellationToken cancellationToken)
    {
        var cacheKey = $"questions:page:{request.Page}";
        var cached = await cacheService.GetAsync<PagedResult<QuestionListItemDto>>(cacheKey, cancellationToken);
        if (cached is not null)
        {
            return cached;
        }

        var search = string.IsNullOrWhiteSpace(request.Search) ? null : request.Search.Trim();
        var tag = string.IsNullOrWhiteSpace(request.Tag) ? null : request.Tag.Trim().ToLowerInvariant();

        var (items, total) = await unitOfWork.Questions.ListAsync(
            new ListQuestionsFilter(search, tag, request.Page, request.PageSize),
            cancellationToken);

        var dtos = items.Select(QuestionMapping.ToListItemDto).ToList();

        var result = new PagedResult<QuestionListItemDto>(dtos, request.Page, request.PageSize, total);
        
        await cacheService.SetAsync(cacheKey, result, TimeSpan.FromMinutes(5), cancellationToken);
        
        return result;
    }
}
