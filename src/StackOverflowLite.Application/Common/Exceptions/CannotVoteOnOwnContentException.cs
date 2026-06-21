namespace StackOverflowLite.Application.Common.Exceptions;

public class CannotVoteOnOwnContentException()
    : Exception("You cannot vote on your own content.");
