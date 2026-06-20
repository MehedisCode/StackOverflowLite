using FluentValidation;

namespace StackOverflowLite.Application.Features.Answers.Queries.GetAnswersByQuestion;

public class GetAnswersByQuestionQueryValidator : AbstractValidator<GetAnswersByQuestionQuery>
{
    public GetAnswersByQuestionQueryValidator()
    {
        RuleFor(x => x.QuestionId)
            .NotEmpty();

        RuleFor(x => x.Page)
            .GreaterThanOrEqualTo(1);

        RuleFor(x => x.PageSize)
            .InclusiveBetween(1, 100);
    }
}
