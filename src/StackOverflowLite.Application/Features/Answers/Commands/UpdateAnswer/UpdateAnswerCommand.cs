using MediatR;
using StackOverflowLite.Application.Features.Answers.DTOs;

namespace StackOverflowLite.Application.Features.Answers.Commands.UpdateAnswer;

public record UpdateAnswerCommand(Guid Id, string Body) : IRequest<AnswerDto>;
