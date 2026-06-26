namespace StackOverflowLite.Domain.Entities;

public class Tag
{
    public Guid Id { get; init; } = Guid.NewGuid();
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public DateTime CreatedAt { get; init; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; private set; }

    public ICollection<QuestionTag> QuestionTags { get; private set; } = [];
}
