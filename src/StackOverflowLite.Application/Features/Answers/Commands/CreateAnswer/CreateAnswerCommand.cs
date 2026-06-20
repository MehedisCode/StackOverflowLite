using MediatR;
using StackOverflowLite.Application.Features.Answers.DTOs;

namespace StackOverflowLite.Application.Features.Answers.Commands.CreateAnswer;

public record CreateAnswerCommand(Guid QuestionId, string Body) : IRequest<AnswerDto>;
