using FluentValidation;

namespace StackOverflowLite.Application.Features.Votes.Commands.RemoveAnswerVote;

public class RemoveAnswerVoteCommandValidator : AbstractValidator<RemoveAnswerVoteCommand>
{
    public RemoveAnswerVoteCommandValidator()
    {
        RuleFor(x => x.AnswerId)
            .NotEmpty();
    }
}
