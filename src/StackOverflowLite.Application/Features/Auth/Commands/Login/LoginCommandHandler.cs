using MediatR;
using Microsoft.Extensions.Options;
using StackOverflowLite.Application.Common.Exceptions;
using StackOverflowLite.Application.Common.Interfaces;
using StackOverflowLite.Application.Common.Options;
using StackOverflowLite.Application.Features.Auth.DTOs;

namespace StackOverflowLite.Application.Features.Auth.Commands.Login;

public class LoginCommandHandler(
    IIdentityService identity,
    IJwtTokenGenerator jwt,
    IOptions<JwtOptions> options) : IRequestHandler<LoginCommand, AuthResponse>
{
    public async Task<AuthResponse> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        var userId = await identity.AuthenticateAsync(request.Email, request.Password, cancellationToken);

        var user = await identity.GetByIdAsync(userId, cancellationToken)
            ?? throw new InvalidCredentialsException();

        var expiresAt = DateTime.UtcNow.AddMinutes(options.Value.AccessTokenExpirationMinutes);
        var token = jwt.Generate(userId, user.Email, user.DisplayName);

        return new AuthResponse(userId, user.Email, user.DisplayName, token, expiresAt);
    }
}
