using MediatR;
using StackOverflowLite.Application.Common.Exceptions;
using StackOverflowLite.Application.Common.Interfaces;
using StackOverflowLite.Application.Features.Tags.Common;
using StackOverflowLite.Domain.Entities;

namespace StackOverflowLite.Application.Features.Tags.Commands.AssignTagToQuestion;

public class AssignTagToQuestionCommandHandler(
    ICurrentUser currentUser,
    IUnitOfWork unitOfWork) : IRequestHandler<AssignTagToQuestionCommand, Unit>
{
    public async Task<Unit> Handle(
        AssignTagToQuestionCommand request, CancellationToken cancellationToken)
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
            throw new ForbiddenAccessException("You can only assign tags to your own questions.");
        }

        var tag = await unitOfWork.Tags.GetByNameAsync(tagName, cancellationToken)
            ?? throw new NotFoundException($"Tag '{tagName}' not found.");

        if (question.QuestionTags.Any(qt => qt.TagId == tag.Id))
        {
            return Unit.Value;
        }

        question.QuestionTags.Add(new QuestionTag
        {
            QuestionId = question.Id,
            TagId = tag.Id,
            Tag = tag
        });

        unitOfWork.Questions.Update(question);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}
