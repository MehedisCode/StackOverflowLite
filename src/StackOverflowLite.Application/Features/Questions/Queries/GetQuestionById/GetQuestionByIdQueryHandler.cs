using MediatR;
using StackOverflowLite.Application.Common.Interfaces;
using StackOverflowLite.Application.Features.Questions.Common;
using StackOverflowLite.Application.Features.Questions.DTOs;

namespace StackOverflowLite.Application.Features.Questions.Queries.GetQuestionById;

public class GetQuestionByIdQueryHandler(
    IUnitOfWork unitOfWork,
    ICacheService cacheService) : IRequestHandler<GetQuestionByIdQuery, QuestionDto?>
{
    public async Task<QuestionDto?> Handle(
        GetQuestionByIdQuery request, CancellationToken cancellationToken)
    {
        var cacheKey = $"question:{request.Id}";
        var cached = await cacheService.GetAsync<QuestionDto>(cacheKey, cancellationToken);
        if (cached is not null)
        {
            return cached;
        }

        var question = await unitOfWork.Questions.GetByIdWithTagsAsync(request.Id, cancellationToken);
        var dto = question is null ? null : QuestionMapping.ToDto(question);

        if (dto is not null)
        {
            await cacheService.SetAsync(cacheKey, dto, TimeSpan.FromMinutes(10), cancellationToken);
        }

        return dto;
    }
}
