namespace StackOverflowLite.Domain.Entities;

public class Answer
{
    public Guid Id { get; init; } = Guid.NewGuid();
    public Guid QuestionId { get; init; }
    public Guid AuthorId { get; init; }
    public string Body { get; set; } = string.Empty;
    public int UpvoteCount { get; private set; }
    public int DownvoteCount { get; private set; }
    public bool IsAccepted { get; private set; }
    public DateTime CreatedAt { get; init; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; private set; }

    public Question Question { get; set; } = null!;

    public void MarkUpdated() => UpdatedAt = DateTime.UtcNow;

    public void MarkAccepted() => IsAccepted = true;

    public void MarkUnaccepted() => IsAccepted = false;

    public void IncrementUpvoteCount() => UpvoteCount++;

    public void DecrementUpvoteCount()
    {
        if (UpvoteCount > 0)
        {
            UpvoteCount--;
        }
    }

    public void IncrementDownvoteCount() => DownvoteCount++;

    public void DecrementDownvoteCount()
    {
        if (DownvoteCount > 0)
        {
            DownvoteCount--;
        }
    }
}
