using Microsoft.Extensions.Logging;
using StackOverflowLite.Application.Common.Exceptions;
using StackOverflowLite.Application.Common.Interfaces;
using StackOverflowLite.Domain.Entities;
using StackOverflowLite.Domain.Enums;

namespace StackOverflowLite.Infrastructure.Services;

public class VoteService(
    IUnitOfWork unitOfWork,
    ILogger<VoteService> logger) : IVoteService
{
    public async Task CastVoteAsync(
        Guid userId, VoteTargetType targetType, Guid targetId, VoteType voteType,
        CancellationToken cancellationToken = default)
    {
        if (targetType == VoteTargetType.Question)
        {
            var question = await unitOfWork.Questions.GetByIdAsync(targetId, cancellationToken)
                ?? throw new NotFoundException($"Question {targetId} not found.");

            if (question.AuthorId == userId)
                throw new CannotVoteOnOwnContentException();

            var (oldVoteType, newVoteType) = await ApplyVoteAsync(
                userId, targetType, targetId, voteType, cancellationToken);

            ApplyCounterMutation(question, oldVoteType, newVoteType);
            if (oldVoteType != newVoteType)
            {
                unitOfWork.Questions.Update(question);
            }
        }
        else
        {
            var answer = await unitOfWork.Answers.GetByIdAsync(targetId, cancellationToken)
                ?? throw new NotFoundException($"Answer {targetId} not found.");

            if (answer.AuthorId == userId)
                throw new CannotVoteOnOwnContentException();

            var (oldVoteType, newVoteType) = await ApplyVoteAsync(
                userId, targetType, targetId, voteType, cancellationToken);

            ApplyCounterMutation(answer, oldVoteType, newVoteType);
            if (oldVoteType != newVoteType)
            {
                unitOfWork.Answers.Update(answer);
            }
        }

        await unitOfWork.SaveChangesAsync(cancellationToken);
    }

    public async Task RemoveVoteAsync(
        Guid userId, VoteTargetType targetType, Guid targetId,
        CancellationToken cancellationToken = default)
    {
        var existing = await unitOfWork.Votes.GetUserVoteForTargetAsync(
            userId, targetId, targetType, cancellationToken);

        if (existing is null)
            return;

        var removedVoteType = existing.VoteType;
        unitOfWork.Votes.Delete(existing);

        if (targetType == VoteTargetType.Question)
        {
            var question = await unitOfWork.Questions.GetByIdAsync(targetId, cancellationToken);
            if (question is not null)
            {
                ApplyCounterMutation(question, removedVoteType, newVoteType: null);
                unitOfWork.Questions.Update(question);
            }
            else
            {
                logger.LogWarning(
                    "Question {QuestionId} missing while reversing vote by user {UserId}.",
                    targetId, userId);
            }
        }
        else
        {
            var answer = await unitOfWork.Answers.GetByIdAsync(targetId, cancellationToken);
            if (answer is not null)
            {
                ApplyCounterMutation(answer, removedVoteType, newVoteType: null);
                unitOfWork.Answers.Update(answer);
            }
            else
            {
                logger.LogWarning(
                    "Answer {AnswerId} missing while reversing vote by user {UserId}.",
                    targetId, userId);
            }
        }

        await unitOfWork.SaveChangesAsync(cancellationToken);
    }

    private async Task<(VoteType? OldVoteType, VoteType NewVoteType)> ApplyVoteAsync(
        Guid userId, VoteTargetType targetType, Guid targetId, VoteType voteType,
        CancellationToken cancellationToken)
    {
        var existing = await unitOfWork.Votes.GetUserVoteForTargetAsync(
            userId, targetId, targetType, cancellationToken);

        if (existing is null)
        {
            await unitOfWork.Votes.AddAsync(new Vote
            {
                UserId = userId,
                TargetId = targetId,
                TargetType = targetType,
                VoteType = voteType
            }, cancellationToken);
            return (null, voteType);
        }

        if (existing.VoteType == voteType)
            return (existing.VoteType, voteType);

        existing.ChangeVoteType(voteType);
        existing.MarkUpdated();
        unitOfWork.Votes.Update(existing);
        return (existing.VoteType, voteType);
    }

    // Apply per-counter mutations on the question/answer entity for the (old, new) vote transition.
    private static void ApplyCounterMutation(Question question, VoteType? oldVoteType, VoteType? newVoteType)
    {
        if (oldVoteType == newVoteType)
            return;

        if (oldVoteType == VoteType.Upvote)
        {
            question.DecrementUpvoteCount();
        }
        else if (oldVoteType == VoteType.Downvote)
        {
            question.DecrementDownvoteCount();
        }

        if (newVoteType == VoteType.Upvote)
        {
            question.IncrementUpvoteCount();
        }
        else if (newVoteType == VoteType.Downvote)
        {
            question.IncrementDownvoteCount();
        }
    }

    private static void ApplyCounterMutation(Answer answer, VoteType? oldVoteType, VoteType? newVoteType)
    {
        if (oldVoteType == newVoteType)
            return;

        if (oldVoteType == VoteType.Upvote)
        {
            answer.DecrementUpvoteCount();
        }
        else if (oldVoteType == VoteType.Downvote)
        {
            answer.DecrementDownvoteCount();
        }

        if (newVoteType == VoteType.Upvote)
        {
            answer.IncrementUpvoteCount();
        }
        else if (newVoteType == VoteType.Downvote)
        {
            answer.IncrementDownvoteCount();
        }
    }
}
