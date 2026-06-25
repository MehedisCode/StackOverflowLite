using MediatR;
using StackOverflowLite.Application.Common.Exceptions;
using StackOverflowLite.Application.Common.Interfaces;
using StackOverflowLite.Application.Features.Questions.Common;
using StackOverflowLite.Application.Features.Questions.DTOs;
using StackOverflowLite.Domain.Entities;

namespace StackOverflowLite.Application.Features.Questions.Commands.CreateQuestion;

public class CreateQuestionCommandHandler(
    ICurrentUser currentUser,
    IUnitOfWork unitOfWork,
    ICacheService cacheService) : IRequestHandler<CreateQuestionCommand, QuestionDto>
{
    public async Task<QuestionDto> Handle(
        CreateQuestionCommand request, CancellationToken cancellationToken)
    {
        if (currentUser.Id is null)
        {
            throw new ForbiddenAccessException("User is not authenticated.");
        }

        var normalizedTags = QuestionMapping.NormalizeTags(request.Tags);

        var existingTags = normalizedTags.Length > 0
            ? await unitOfWork.Tags.GetByNamesAsync(normalizedTags, cancellationToken)
            : [];

        var existingNames = existingTags.Select(t => t.Name).ToHashSet();
        var newTags = normalizedTags
            .Where(n => !existingNames.Contains(n))
            .Select(n => new Tag { Name = n })
            .ToList();

        foreach (var t in newTags)
        {
            await unitOfWork.Tags.AddAsync(t, cancellationToken);
        }

        var question = new Question
        {
            AuthorId = currentUser.Id.Value,
            Title = request.Title.Trim(),
            Body = request.Body,
        };

        foreach (var tag in existingTags.Concat(newTags))
        {
            question.QuestionTags.Add(new QuestionTag
            {
                QuestionId = question.Id,
                TagId = tag.Id,
                Tag = tag
            });
        }

        await unitOfWork.Questions.AddAsync(question, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        await cacheService.RemoveAsync($"user:profile:{currentUser.Id.Value}", cancellationToken);

        return QuestionMapping.ToDto(question);
    }
}
