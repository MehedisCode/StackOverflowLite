using MediatR;
using StackOverflowLite.Application.Features.Auth.DTOs;

namespace StackOverflowLite.Application.Features.Auth.Commands.Login;

public record LoginCommand(string Email, string Password)
    : IRequest<AuthResponse>;
