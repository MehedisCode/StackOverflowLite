using MediatR;
using StackOverflowLite.Application.Features.Questions.DTOs;

namespace StackOverflowLite.Application.Features.Questions.Commands.UpdateQuestion;

public record UpdateQuestionCommand(
    Guid Id,
    string Title,
    string Body,
    string[] Tags) : IRequest<QuestionDto>;
