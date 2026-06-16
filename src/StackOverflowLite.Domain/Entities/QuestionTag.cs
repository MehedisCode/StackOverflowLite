namespace StackOverflowLite.Domain.Entities;

public class QuestionTag
{
    public Guid QuestionId { get; init; }
    public Guid TagId { get; init; }
    public DateTime CreatedAt { get; init; } = DateTime.UtcNow;

    public Question Question { get; set; } = null!;
    public Tag Tag { get; set; } = null!;
}
