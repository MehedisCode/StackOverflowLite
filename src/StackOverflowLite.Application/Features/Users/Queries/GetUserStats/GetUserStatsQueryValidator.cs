using FluentValidation;

namespace StackOverflowLite.Application.Features.Users.Queries.GetUserStats;

public class GetUserStatsQueryValidator : AbstractValidator<GetUserStatsQuery>
{
    public GetUserStatsQueryValidator()
    {
        RuleFor(x => x.UserId)
            .NotEmpty();
    }
}
