using Microsoft.AspNetCore.Identity;
using StackOverflowLite.Application.Common.Exceptions;
using StackOverflowLite.Application.Common.Interfaces;
using StackOverflowLite.Application.Features.Auth.DTOs;
using StackOverflowLite.Infrastructure.Identity;

namespace StackOverflowLite.Infrastructure.Services;

public class IdentityService(UserManager<ApplicationUser> userManager) : IIdentityService
{
    public async Task<Guid> CreateUserAsync(
        string email,
        string password,
        string displayName,
        CancellationToken cancellationToken = default)
    {
        var user = new ApplicationUser
        {
            UserName = email,
            Email = email,
            DisplayName = displayName
        };

        var result = await userManager.CreateAsync(user, password);
        if (!result.Succeeded)
        {
            if (result.Errors.Any(e =>
                e.Code.Equals("DuplicateEmail", StringComparison.OrdinalIgnoreCase) ||
                e.Code.Equals("DuplicateUserName", StringComparison.OrdinalIgnoreCase)))
            {
                throw new EmailAlreadyExistsException(email);
            }

            var description = string.Join("; ", result.Errors.Select(e => e.Description));
            throw new InvalidOperationException($"User creation failed: {description}");
        }

        return user.Id;
    }

    public async Task<Guid> AuthenticateAsync(
        string email,
        string password,
        CancellationToken cancellationToken = default)
    {
        var user = await userManager.FindByEmailAsync(email);
        if (user is null)
        {
            throw new InvalidCredentialsException();
        }

        var ok = await userManager.CheckPasswordAsync(user, password);
        if (!ok)
        {
            throw new InvalidCredentialsException();
        }

        return user.Id;
    }

    public async Task<CurrentUserDto?> GetByIdAsync(
        Guid userId,
        CancellationToken cancellationToken = default)
    {
        var user = await userManager.FindByIdAsync(userId.ToString());
        if (user is null)
        {
            return null;
        }

        return new CurrentUserDto(
            user.Id,
            user.Email ?? string.Empty,
            user.DisplayName,
            user.Bio,
            user.AvatarUrl,
            user.Reputation,
            user.CreatedAt);
    }
}
