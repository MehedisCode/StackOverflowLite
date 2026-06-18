namespace StackOverflowLite.Application.Common.Exceptions;

public class EmailAlreadyExistsException(string email)
    : Exception($"A user with email '{email}' already exists.");
