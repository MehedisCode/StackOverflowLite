using MediatR;
using StackOverflowLite.Application.Common.Interfaces;
using StackOverflowLite.Application.Features.Users.DTOs;

namespace StackOverflowLite.Application.Features.Users.Queries.GetUserProfile;

public class GetUserProfileQueryHandler(
    IIdentityService identityService,
    IUnitOfWork unitOfWork,
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

        var user = await identityService.GetByIdAsync(userId, cancellationToken);
        if (user is null) return null;

        var questionsCount = await unitOfWork.Questions.GetUserQuestionCountAsync(userId, cancellationToken);
        var answersCount = await unitOfWork.Answers.GetUserAnswerCountAsync(userId, cancellationToken);
        var q = await unitOfWork.Questions.GetUserQuestionVoteStatsAsync(userId, cancellationToken);
        var a = await unitOfWork.Answers.GetUserAnswerVoteStatsAsync(userId, cancellationToken);

        return new UserProfileDto(
            UserId: user.Id,
            DisplayName: user.DisplayName,
            JoinedAt: user.CreatedAt,
            QuestionsCount: questionsCount,
            AnswersCount: answersCount,
            QuestionUpvotesReceived: q.Upvotes,
            QuestionDownvotesReceived: q.Downvotes,
            AnswerUpvotesReceived: a.Upvotes,
            AnswerDownvotesReceived: a.Downvotes,
            AcceptedAnswersCount: a.Accepted);
    }
}
