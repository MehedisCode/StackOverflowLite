namespace StackOverflowLite.Application.Common.Interfaces;

public interface ICurrentUser
{
    Guid? Id { get; }
    bool IsAuthenticated { get; }
}
