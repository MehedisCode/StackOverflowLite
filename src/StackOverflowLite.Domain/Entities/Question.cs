namespace StackOverflowLite.Domain.Entities;

public class Question
{
    public Guid Id { get; init; } = Guid.NewGuid();
    public Guid AuthorId { get; init; }
    public string Title { get; set; } = string.Empty;
    public string Body { get; set; } = string.Empty;
    public int UpvoteCount { get; private set; }
    public int DownvoteCount { get; private set; }
    public int AnswerCount { get; private set; }
    public int ViewCount { get; private set; }
    public Guid? AcceptedAnswerId { get; private set; }

    public DateTime CreatedAt { get; init; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; private set; }

    public ICollection<Answer> Answers { get; private set; } = [];
    public ICollection<QuestionTag> QuestionTags { get; private set; } = [];
    public Answer? AcceptedAnswer { get; set; }

    public void MarkUpdated() => UpdatedAt = DateTime.UtcNow;

    public void SetAcceptedAnswer(Guid? id) => AcceptedAnswerId = id;

    public void ClearAcceptedAnswer() => AcceptedAnswerId = null;

    public void IncrementViewCount() => ViewCount++;

    public void IncrementAnswerCount() => AnswerCount++;

    public void DecrementAnswerCount()
    {
        if (AnswerCount > 0)
        {
            AnswerCount--;
        }
    }

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
