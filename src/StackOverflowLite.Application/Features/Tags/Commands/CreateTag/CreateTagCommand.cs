using MediatR;
using StackOverflowLite.Application.Features.Tags.DTOs;

namespace StackOverflowLite.Application.Features.Tags.Commands.CreateTag;

public record CreateTagCommand(string Name, string? Description) : IRequest<TagDto>;
