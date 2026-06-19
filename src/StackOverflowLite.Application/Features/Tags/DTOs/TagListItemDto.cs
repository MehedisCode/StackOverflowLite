namespace StackOverflowLite.Application.Features.Tags.DTOs;

public record TagListItemDto(
    Guid Id,
    string Name,
    string? Description,
    int UsageCount);
