using MediatR;
using Microsoft.Extensions.Options;
using StackOverflowLite.Application.Common.Interfaces;
using StackOverflowLite.Application.Common.Options;
using StackOverflowLite.Application.Features.Auth.DTOs;

namespace StackOverflowLite.Application.Features.Auth.Commands.Register;

public class RegisterCommandHandler(
    IIdentityService identity,
    IJwtTokenGenerator jwt,
    IOptions<JwtOptions> options) : IRequestHandler<RegisterCommand, AuthResponse>
{
    public async Task<AuthResponse> Handle(RegisterCommand request, CancellationToken cancellationToken)
    {
        var userId = await identity.CreateUserAsync(
            request.Email, request.Password, request.DisplayName, cancellationToken);

        var expiresAt = DateTime.UtcNow.AddMinutes(options.Value.AccessTokenExpirationMinutes);
        var token = jwt.Generate(userId, request.Email, request.DisplayName);

        return new AuthResponse(userId, request.Email, request.DisplayName, token, expiresAt);
    }
}
