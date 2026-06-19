namespace StackOverflowLite.Application.Features.Auth.DTOs;

public record CurrentUserDto(
    Guid Id,
    string Email,
    string DisplayName,
    int Reputation,
    DateTime CreatedAt);
