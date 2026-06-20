using MediatR;
using StackOverflowLite.Application.Common.Exceptions;
using StackOverflowLite.Application.Common.Interfaces;
using StackOverflowLite.Application.Features.Answers.Common;
using StackOverflowLite.Application.Features.Answers.DTOs;
using StackOverflowLite.Domain.Entities;

namespace StackOverflowLite.Application.Features.Answers.Commands.CreateAnswer;

public class CreateAnswerCommandHandler(
    ICurrentUser currentUser,
    IUnitOfWork unitOfWork) : IRequestHandler<CreateAnswerCommand, AnswerDto>
{
    public async Task<AnswerDto> Handle(
        CreateAnswerCommand request, CancellationToken cancellationToken)
    {
        if (currentUser.Id is null)
        {
            throw new ForbiddenAccessException("User is not authenticated.");
        }

        var question = await unitOfWork.Questions.GetByIdAsync(request.QuestionId, cancellationToken)
            ?? throw new NotFoundException($"Question {request.QuestionId} not found.");

        var answer = new Answer
        {
            QuestionId = request.QuestionId,
            AuthorId = currentUser.Id.Value,
            Body = request.Body
        };

        await unitOfWork.Answers.AddAsync(answer, cancellationToken);

        question.IncrementAnswerCount();
        unitOfWork.Questions.Update(question);

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return AnswerMapping.ToDto(answer);
    }
}
