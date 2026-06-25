using MediatR;
using StackOverflowLite.Application.Common.Interfaces;
using StackOverflowLite.Application.Features.Users.DTOs;

namespace StackOverflowLite.Application.Features.Users.Queries.GetUserProfile;

public class GetUserProfileQueryHandler(
    IIdentityService identityService,
    IUnitOfWork unitOfWork,
    ICacheService cacheService,
    ICurrentUser currentUser) : IRequestHandler<GetUserProfileQuery, UserProfileDto?>
{
    public async Task<UserProfileDto?> Handle(
        GetUserProfileQuery request, CancellationToken cancellationToken)
    {
        if (!currentUser.IsAuthenticated || currentUser.Id is null)
        {
            return null;
        }

        var userId = currentUser.Id.Value;

        var cacheKey = $"user:profile:{userId}";
        var cached = await cacheService.GetAsync<UserProfileDto>(cacheKey, cancellationToken);
        if (cached is not null)
        {
            return cached;
        }

        var user = await identityService.GetByIdAsync(userId, cancellationToken);
        if (user is null) return null;

        var questionsCount = await unitOfWork.Questions.GetUserQuestionCountAsync(userId, cancellationToken);
        var answersCount = await unitOfWork.Answers.GetUserAnswerCountAsync(userId, cancellationToken);
        var q = await unitOfWork.Questions.GetUserQuestionVoteStatsAsync(userId, cancellationToken);
        var a = await unitOfWork.Answers.GetUserAnswerVoteStatsAsync(userId, cancellationToken);

        var dto = new UserProfileDto(
            UserId: user.Id,
            DisplayName: user.DisplayName,
            Email: user.Email,
            JoinedAt: user.CreatedAt,
            QuestionsCount: questionsCount,
            AnswersCount: answersCount,
            QuestionUpvotesReceived: q.Upvotes,
            QuestionDownvotesReceived: q.Downvotes,
            AnswerUpvotesReceived: a.Upvotes,
            AnswerDownvotesReceived: a.Downvotes,
            AcceptedAnswersCount: a.Accepted);

        await cacheService.SetAsync(cacheKey, dto, TimeSpan.FromMinutes(5), cancellationToken);

        return dto;
    }
}
