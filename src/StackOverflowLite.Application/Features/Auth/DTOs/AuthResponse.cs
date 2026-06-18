namespace StackOverflowLite.Application.Features.Auth.DTOs;

public record AuthResponse(
    Guid UserId,
    string Email,
    string DisplayName,
    string AccessToken,
    DateTime ExpiresAt);
