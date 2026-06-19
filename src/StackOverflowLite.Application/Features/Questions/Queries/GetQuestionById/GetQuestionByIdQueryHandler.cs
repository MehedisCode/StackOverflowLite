using MediatR;
using StackOverflowLite.Application.Common.Interfaces;
using StackOverflowLite.Application.Features.Questions.Common;
using StackOverflowLite.Application.Features.Questions.DTOs;

namespace StackOverflowLite.Application.Features.Questions.Queries.GetQuestionById;

public class GetQuestionByIdQueryHandler(
    IUnitOfWork unitOfWork) : IRequestHandler<GetQuestionByIdQuery, QuestionDto?>
{
    public async Task<QuestionDto?> Handle(
        GetQuestionByIdQuery request, CancellationToken cancellationToken)
    {
        var question = await unitOfWork.Questions.GetByIdWithTagsAsync(request.Id, cancellationToken);
        return question is null ? null : QuestionMapping.ToDto(question);
    }
}
