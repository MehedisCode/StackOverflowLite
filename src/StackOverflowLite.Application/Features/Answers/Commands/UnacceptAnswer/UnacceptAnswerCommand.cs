using MediatR;

namespace StackOverflowLite.Application.Features.Answers.Commands.UnacceptAnswer;

public record UnacceptAnswerCommand(Guid AnswerId) : IRequest<Unit>;