using MediatR;
using StackOverflowLite.Application.Common.Exceptions;
using StackOverflowLite.Application.Common.Interfaces;
using StackOverflowLite.Application.Common.Models;
using StackOverflowLite.Application.Features.Answers.Common;
using StackOverflowLite.Application.Features.Answers.DTOs;

namespace StackOverflowLite.Application.Features.Answers.Queries.GetAnswersByQuestion;

public class GetAnswersByQuestionQueryHandler(IUnitOfWork unitOfWork)
    : IRequestHandler<GetAnswersByQuestionQuery, PagedResult<AnswerListItemDto>>
{
    public async Task<PagedResult<AnswerListItemDto>> Handle(
        GetAnswersByQuestionQuery request, CancellationToken cancellationToken)
    {
        var question = await unitOfWork.Questions.GetByIdAsync(request.QuestionId, cancellationToken)
            ?? throw new NotFoundException($"Question {request.QuestionId} not found.");

        var (items, total) = await unitOfWork.Answers.ListAsync(
            new ListAnswersFilter(request.QuestionId, request.Page, request.PageSize),
            cancellationToken);

        var dtos = items.Select(AnswerMapping.ToListItemDto).ToList();

        return new PagedResult<AnswerListItemDto>(
            dtos, request.Page, request.PageSize, total);
    }
}
