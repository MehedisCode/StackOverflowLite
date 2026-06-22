using MediatR;
using StackOverflowLite.Application.Common.Interfaces;
using StackOverflowLite.Application.Features.Users.DTOs;

namespace StackOverflowLite.Application.Features.Users.Queries.GetUserStats;

public class GetUserStatsQueryHandler(
    IIdentityService identityService,
    IUnitOfWork unitOfWork) : IRequestHandler<GetUserStatsQuery, UserStatsDto?>
{
    public async Task<UserStatsDto?> Handle(
        GetUserStatsQuery request, CancellationToken cancellationToken)
    {
        var user = await identityService.GetByIdAsync(request.UserId, cancellationToken);
        if (user is null) return null;

        var q = await unitOfWork.Questions.GetUserQuestionVoteStatsAsync(request.UserId, cancellationToken);
        var a = await unitOfWork.Answers.GetUserAnswerVoteStatsAsync(request.UserId, cancellationToken);

        return new UserStatsDto(
            QuestionUpvotesReceived: q.Upvotes,
            QuestionDownvotesReceived: q.Downvotes,
            AnswerUpvotesReceived: a.Upvotes,
            AnswerDownvotesReceived: a.Downvotes,
            AcceptedAnswersCount: a.Accepted);
    }
}
