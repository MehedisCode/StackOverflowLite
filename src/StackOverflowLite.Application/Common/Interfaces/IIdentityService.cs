using StackOverflowLite.Application.Features.Auth.DTOs;

namespace StackOverflowLite.Application.Common.Interfaces;

public interface IIdentityService
{
    Task<Guid> CreateUserAsync(string email, string password, string displayName, CancellationToken cancellationToken = default);

    Task<Guid> AuthenticateAsync(string email, string password, CancellationToken cancellationToken = default);

    Task<CurrentUserDto?> GetByIdAsync(Guid userId, CancellationToken cancellationToken = default);
}
