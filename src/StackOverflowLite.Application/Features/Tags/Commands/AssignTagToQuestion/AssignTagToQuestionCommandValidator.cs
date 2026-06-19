using FluentValidation;

namespace StackOverflowLite.Application.Features.Tags.Commands.AssignTagToQuestion;

public class AssignTagToQuestionCommandValidator : AbstractValidator<AssignTagToQuestionCommand>
{
    public AssignTagToQuestionCommandValidator()
    {
        RuleFor(x => x.QuestionId)
            .NotEmpty();

        RuleFor(x => x.TagName)
            .NotEmpty()
            .MaximumLength(25);
    }
}
