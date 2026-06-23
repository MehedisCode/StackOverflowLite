using MediatR;
using StackOverflowLite.Application.Common.Exceptions;
using StackOverflowLite.Application.Common.Interfaces;
using StackOverflowLite.Application.Features.Questions.Common;
using StackOverflowLite.Application.Features.Questions.DTOs;
using StackOverflowLite.Domain.Entities;

namespace StackOverflowLite.Application.Features.Questions.Commands.UpdateQuestion;

public class UpdateQuestionCommandHandler(
    ICurrentUser currentUser,
    IUnitOfWork unitOfWork,
    ICacheService cacheService) : IRequestHandler<UpdateQuestionCommand, QuestionDto>
{
    public async Task<QuestionDto> Handle(
        UpdateQuestionCommand request, CancellationToken cancellationToken)
    {
        if (currentUser.Id is null)
        {
            throw new ForbiddenAccessException("User is not authenticated.");
        }

        var question = await unitOfWork.Questions.GetByIdWithTagsAsync(request.Id, cancellationToken)
            ?? throw new NotFoundException($"Question {request.Id} not found.");

        if (question.AuthorId != currentUser.Id.Value)
        {
            throw new ForbiddenAccessException("You can only edit your own questions.");
        }

        question.Title = request.Title.Trim();
        question.Body = request.Body;

        var normalizedTags = QuestionMapping.NormalizeTags(request.Tags);

        var existingTags = normalizedTags.Length > 0
            ? await unitOfWork.Tags.GetByNamesAsync(normalizedTags, cancellationToken)
            : new List<Tag>();

        var existingNames = existingTags.Select(t => t.Name).ToHashSet();
        var newTags = normalizedTags
            .Where(n => !existingNames.Contains(n))
            .Select(n => new Tag { Name = n })
            .ToList();

        foreach (var t in newTags)
        {
            await unitOfWork.Tags.AddAsync(t, cancellationToken);
        }

        question.QuestionTags.Clear();
        foreach (var tag in existingTags.Concat(newTags))
        {
            question.QuestionTags.Add(new QuestionTag
            {
                QuestionId = question.Id,
                TagId = tag.Id,
                Tag = tag
            });
        }

        question.MarkUpdated();
        unitOfWork.Questions.Update(question);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        await cacheService.RemoveAsync($"question:{request.Id}", cancellationToken);
        await cacheService.RemoveByPrefixAsync("questions:page:", cancellationToken);

        return QuestionMapping.ToDto(question);
    }
}
