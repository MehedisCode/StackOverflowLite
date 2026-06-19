using MediatR;
using StackOverflowLite.Application.Common.Exceptions;
using StackOverflowLite.Application.Common.Interfaces;
using StackOverflowLite.Application.Features.Tags.Common;

namespace StackOverflowLite.Application.Features.Tags.Commands.RemoveTagFromQuestion;

public class RemoveTagFromQuestionCommandHandler(
    ICurrentUser currentUser,
    IUnitOfWork unitOfWork) : IRequestHandler<RemoveTagFromQuestionCommand, Unit>
{
    public async Task<Unit> Handle(
        RemoveTagFromQuestionCommand request, CancellationToken cancellationToken)
    {
        if (currentUser.Id is null)
        {
            throw new ForbiddenAccessException("User is not authenticated.");
        }

        var tagName = TagMapping.NormalizeName(request.TagName);

        var question = await unitOfWork.Questions.GetByIdWithTagsAsync(request.QuestionId, cancellationToken)
            ?? throw new NotFoundException($"Question {request.QuestionId} not found.");

        if (question.AuthorId != currentUser.Id.Value)
        {
            throw new ForbiddenAccessException("You can only remove tags from your own questions.");
        }

        var link = question.QuestionTags.FirstOrDefault(qt => qt.Tag.Name == tagName);

        if (link is null)
        {
            return Unit.Value;
        }

        question.QuestionTags.Remove(link);

        unitOfWork.Questions.Update(question);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}
