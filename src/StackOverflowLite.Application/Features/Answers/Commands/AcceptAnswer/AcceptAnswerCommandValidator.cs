using FluentValidation;

namespace StackOverflowLite.Application.Features.Answers.Commands.AcceptAnswer;

public class AcceptAnswerCommandValidator : AbstractValidator<AcceptAnswerCommand>
{
    public AcceptAnswerCommandValidator()
    {
        RuleFor(x => x.AnswerId)
            .NotEmpty();
    }
}