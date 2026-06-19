namespace StackOverflowLite.Application.Common.Exceptions;

public class DuplicateTagException(string name)
    : Exception($"A tag with name '{name}' already exists.");
