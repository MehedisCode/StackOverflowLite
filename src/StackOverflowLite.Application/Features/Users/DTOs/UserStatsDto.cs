namespace StackOverflowLite.Application.Features.Users.DTOs;

public record UserStatsDto(
    int QuestionUpvotesReceived,
    int QuestionDownvotesReceived,
    int AnswerUpvotesReceived,
    int AnswerDownvotesReceived,
    int AcceptedAnswersCount);
