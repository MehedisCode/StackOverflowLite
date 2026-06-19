using FluentValidation;

namespace StackOverflowLite.Application.Features.Tags.Queries.GetTags;

public class GetTagsQueryValidator : AbstractValidator<GetTagsQuery>
{
    public GetTagsQueryValidator()
    {
        RuleFor(x => x.Page)
            .GreaterThanOrEqualTo(1);

        RuleFor(x => x.PageSize)
            .InclusiveBetween(1, 100);

        RuleFor(x => x.Search)
            .MaximumLength(100)
            .When(x => x.Search is not null);
    }
}
