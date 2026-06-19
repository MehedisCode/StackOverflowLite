namespace StackOverflowLite.Application.Features.Questions.DTOs;

public record CreateQuestionRequest(
    string Title,
    string Body,
    string[] Tags);
