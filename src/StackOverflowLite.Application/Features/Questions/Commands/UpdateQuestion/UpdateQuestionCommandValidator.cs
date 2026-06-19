using FluentValidation;

namespace StackOverflowLite.Application.Features.Questions.Commands.UpdateQuestion;

public class UpdateQuestionCommandValidator : AbstractValidator<UpdateQuestionCommand>
{
    public UpdateQuestionCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty();

        RuleFor(x => x.Title)
            .NotEmpty()
            .MinimumLength(5)
            .MaximumLength(150);

        RuleFor(x => x.Body)
            .NotEmpty()
            .MinimumLength(5);

        RuleFor(x => x.Tags)
            .NotNull()
            .Must(t => t.Length <= 5)
                .WithMessage("At most 5 tags are allowed.");

        RuleForEach(x => x.Tags)
            .NotEmpty()
            .MaximumLength(25);
    }
}
