using FluentValidation;

namespace StackOverflowLite.Application.Features.Votes.Commands.DownvoteQuestion;

public class DownvoteQuestionCommandValidator : AbstractValidator<DownvoteQuestionCommand>
{
    public DownvoteQuestionCommandValidator()
    {
        RuleFor(x => x.QuestionId)
            .NotEmpty();
    }
}
