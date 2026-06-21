using FluentValidation;

namespace StackOverflowLite.Application.Features.Votes.Commands.DownvoteAnswer;

public class DownvoteAnswerCommandValidator : AbstractValidator<DownvoteAnswerCommand>
{
    public DownvoteAnswerCommandValidator()
    {
        RuleFor(x => x.AnswerId)
            .NotEmpty();
    }
}
