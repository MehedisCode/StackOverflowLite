using FluentValidation;

namespace StackOverflowLite.Application.Features.Votes.Commands.RemoveQuestionVote;

public class RemoveQuestionVoteCommandValidator : AbstractValidator<RemoveQuestionVoteCommand>
{
    public RemoveQuestionVoteCommandValidator()
    {
        RuleFor(x => x.QuestionId)
            .NotEmpty();
    }
}
