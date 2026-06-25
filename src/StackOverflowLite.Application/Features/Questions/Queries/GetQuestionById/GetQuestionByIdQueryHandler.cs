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
            var question = await unitOfWork.Questions.GetByIdAsync(request.Id, cancellationToken);
            if (question is not null)
            {
                question.IncrementViewCount();
                unitOfWork.Questions.Update(question);
                await unitOfWork.SaveChangesAsync(cancellationToken);
            }

            var updatedDto = cached with { ViewCount = cached.ViewCount + 1 };
            await cacheService.SetAsync(cacheKey, updatedDto, TimeSpan.FromMinutes(10), cancellationToken);

            return updatedDto;
        }

        var fullQuestion = await unitOfWork.Questions.GetByIdWithTagsAsync(request.Id, cancellationToken);
        if (fullQuestion is null)
        {
            return null;
        }

        fullQuestion.IncrementViewCount();
        unitOfWork.Questions.Update(fullQuestion);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        var dto = QuestionMapping.ToDto(fullQuestion);
        await cacheService.SetAsync(cacheKey, dto, TimeSpan.FromMinutes(10), cancellationToken);

        return dto;
    }
}
