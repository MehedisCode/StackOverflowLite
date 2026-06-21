using FluentValidation;

namespace StackOverflowLite.Application.Features.Answers.Commands.UnacceptAnswer;

public class UnacceptAnswerCommandValidator : AbstractValidator<UnacceptAnswerCommand>
{
    public UnacceptAnswerCommandValidator()
    {
        RuleFor(x => x.AnswerId)
            .NotEmpty();
    }
}