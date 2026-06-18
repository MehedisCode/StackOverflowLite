using MediatR;
using StackOverflowLite.Application.Features.Auth.DTOs;

namespace StackOverflowLite.Application.Features.Auth.Commands.Register;

public record RegisterCommand(string Email, string Password, string DisplayName)
    : IRequest<AuthResponse>;
