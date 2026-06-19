using MediatR;
using StackOverflowLite.Application.Common.Interfaces;
using StackOverflowLite.Application.Common.Models;
using StackOverflowLite.Application.Features.Questions.Common;
using StackOverflowLite.Application.Features.Questions.DTOs;
using StackOverflowLite.Application.Features.Tags.Common;

namespace StackOverflowLite.Application.Features.Tags.Queries.GetQuestionsByTag;

public class GetQuestionsByTagQueryHandler(IUnitOfWork unitOfWork)
    : IRequestHandler<GetQuestionsByTagQuery, PagedResult<QuestionListItemDto>>
{
    public async Task<PagedResult<QuestionListItemDto>> Handle(
        GetQuestionsByTagQuery request, CancellationToken cancellationToken)
    {
        var tagName = TagMapping.NormalizeName(request.TagName);

        var (items, total) = await unitOfWork.Questions.ListAsync(
            new ListQuestionsFilter(Search: null, TagName: tagName, request.Page, request.PageSize),
            cancellationToken);

        var mapped = items.Select(QuestionMapping.ToListItemDto).ToList();

        return new PagedResult<QuestionListItemDto>(
            mapped, request.Page, request.PageSize, total);
    }
}
