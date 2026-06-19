namespace StackOverflowLite.Application.Features.Tags.DTOs;

public record TagDto(
    Guid Id,
    string Name,
    string? Description,
    int UsageCount,
    DateTime CreatedAt);
