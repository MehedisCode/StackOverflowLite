using FluentValidation;

namespace StackOverflowLite.Application.Features.Votes.Commands.UpvoteAnswer;

public class UpvoteAnswerCommandValidator : AbstractValidator<UpvoteAnswerCommand>
{
    public UpvoteAnswerCommandValidator()
    {
        RuleFor(x => x.AnswerId)
            .NotEmpty();
    }
}
