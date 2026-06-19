using MediatR;
using StackOverflowLite.Application.Features.Questions.DTOs;

namespace StackOverflowLite.Application.Features.Questions.Commands.CreateQuestion;

public record CreateQuestionCommand(
    string Title,
    string Body,
    string[] Tags) : IRequest<QuestionDto>;
