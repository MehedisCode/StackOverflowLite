namespace StackOverflowLite.Application.Features.Users.DTOs;

public record UserProfileDto(
    Guid UserId,
    string DisplayName,
    DateTime JoinedAt,
    int QuestionsCount,
    int AnswersCount,
    int QuestionUpvotesReceived,
    int QuestionDownvotesReceived,
    int AnswerUpvotesReceived,
    int AnswerDownvotesReceived,
    int AcceptedAnswersCount);
