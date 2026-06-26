using StackOverflowLite.Application.Features.Tags.DTOs;
using StackOverflowLite.Domain.Entities;

namespace StackOverflowLite.Application.Features.Tags.Common;

internal static class TagMapping
{
    public static string NormalizeName(string name) =>
        name.Trim().ToLowerInvariant();

    public static TagDto ToDto(Tag tag) =>
        new(tag.Id, tag.Name, tag.Description, tag.CreatedAt);

    public static TagListItemDto ToListItemDto(Tag tag) =>
        new(tag.Id, tag.Name, tag.Description);
}
