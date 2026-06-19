namespace StackOverflowLite.Application.Features.Questions.DTOs;

public record UpdateQuestionRequest(
    string Title,
    string Body,
    string[] Tags);
