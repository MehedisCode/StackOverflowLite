using FluentValidation;

namespace StackOverflowLite.Application.Features.Votes.Commands.UpvoteQuestion;

public class UpvoteQuestionCommandValidator : AbstractValidator<UpvoteQuestionCommand>
{
    public UpvoteQuestionCommandValidator()
    {
        RuleFor(x => x.QuestionId)
            .NotEmpty();
    }
}
