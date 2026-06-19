using FluentValidation;

namespace StackOverflowLite.Application.Features.Tags.Queries.GetQuestionsByTag;

public class GetQuestionsByTagQueryValidator : AbstractValidator<GetQuestionsByTagQuery>
{
    public GetQuestionsByTagQueryValidator()
    {
        RuleFor(x => x.TagName)
            .NotEmpty()
            .MaximumLength(25);

        RuleFor(x => x.Page)
            .GreaterThanOrEqualTo(1);

        RuleFor(x => x.PageSize)
            .InclusiveBetween(1, 100);
    }
}
