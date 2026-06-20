using MediatR;
using StackOverflowLite.Application.Common.Exceptions;
using StackOverflowLite.Application.Common.Interfaces;
using StackOverflowLite.Application.Features.Answers.Common;
using StackOverflowLite.Application.Features.Answers.DTOs;

namespace StackOverflowLite.Application.Features.Answers.Commands.UpdateAnswer;

public class UpdateAnswerCommandHandler(
    ICurrentUser currentUser,
    IUnitOfWork unitOfWork) : IRequestHandler<UpdateAnswerCommand, AnswerDto>
{
    public async Task<AnswerDto> Handle(
        UpdateAnswerCommand request, CancellationToken cancellationToken)
    {
        if (currentUser.Id is null)
        {
            throw new ForbiddenAccessException("User is not authenticated.");
        }

        var answer = await unitOfWork.Answers.GetByIdAsync(request.Id, cancellationToken)
            ?? throw new NotFoundException($"Answer {request.Id} not found.");

        if (answer.AuthorId != currentUser.Id.Value)
        {
            throw new ForbiddenAccessException("You can only edit your own answers.");
        }

        answer.Body = request.Body;
        answer.MarkUpdated();

        unitOfWork.Answers.Update(answer);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return AnswerMapping.ToDto(answer);
    }
}
