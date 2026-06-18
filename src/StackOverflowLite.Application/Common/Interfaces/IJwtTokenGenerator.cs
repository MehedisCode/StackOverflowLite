namespace StackOverflowLite.Application.Common.Interfaces;

public interface IJwtTokenGenerator
{
    string Generate(Guid userId, string email, string displayName);
}
