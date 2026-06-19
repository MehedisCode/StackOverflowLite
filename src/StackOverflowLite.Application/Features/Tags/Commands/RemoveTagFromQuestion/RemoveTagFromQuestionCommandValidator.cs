using FluentValidation;

namespace StackOverflowLite.Application.Features.Tags.Commands.RemoveTagFromQuestion;

public class RemoveTagFromQuestionCommandValidator : AbstractValidator<RemoveTagFromQuestionCommand>
{
    public RemoveTagFromQuestionCommandValidator()
    {
        RuleFor(x => x.QuestionId)
            .NotEmpty();

        RuleFor(x => x.TagName)
            .NotEmpty()
            .MaximumLength(25);
    }
}
