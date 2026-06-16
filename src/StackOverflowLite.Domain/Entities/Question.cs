namespace StackOverflowLite.Domain.Entities;

public class Question
{
    public Guid Id { get; init; } = Guid.NewGuid();
    public Guid AuthorId { get; init; }
    public string Title { get; set; } = string.Empty;
    public string Body { get; set; } = string.Empty;
    public int Score { get; private set; }
    public long ViewCount { get; private set; }
    public int AnswerCount { get; private set; }
    public Guid? AcceptedAnswerId { get; init; }
    public DateTime CreatedAt { get; init; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; private set; }

    public ICollection<Answer> Answers { get; private set; } = [];
    public ICollection<QuestionTag> QuestionTags { get; private set; } = [];
    public Answer? AcceptedAnswer { get; set; }
}
