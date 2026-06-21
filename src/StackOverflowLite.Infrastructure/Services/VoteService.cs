using StackOverflowLite.Application.Common.Exceptions;
using StackOverflowLite.Application.Common.Interfaces;
using StackOverflowLite.Domain.Entities;
using StackOverflowLite.Domain.Enums;

namespace StackOverflowLite.Infrastructure.Services;

public class VoteService(IUnitOfWork unitOfWork) : IVoteService
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

            var delta = await ApplyVoteAsync(userId, targetType, targetId, voteType, cancellationToken);
            question.IncrementScore(delta);
            unitOfWork.Questions.Update(question);
        }
        else
        {
            var answer = await unitOfWork.Answers.GetByIdAsync(targetId, cancellationToken)
                ?? throw new NotFoundException($"Answer {targetId} not found.");

            if (answer.AuthorId == userId)
                throw new CannotVoteOnOwnContentException();

            var delta = await ApplyVoteAsync(userId, targetType, targetId, voteType, cancellationToken);
            answer.IncrementScore(delta);
            unitOfWork.Answers.Update(answer);
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

        var delta = -(int)existing.VoteType;
        unitOfWork.Votes.Delete(existing);

        if (targetType == VoteTargetType.Question)
        {
            var question = await unitOfWork.Questions.GetByIdAsync(targetId, cancellationToken);
            if (question is not null)
            {
                question.IncrementScore(delta);
                unitOfWork.Questions.Update(question);
            }
        }
        else
        {
            var answer = await unitOfWork.Answers.GetByIdAsync(targetId, cancellationToken);
            if (answer is not null)
            {
                answer.IncrementScore(delta);
                unitOfWork.Answers.Update(answer);
            }
        }

        await unitOfWork.SaveChangesAsync(cancellationToken);
    }

    private async Task<int> ApplyVoteAsync(
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
            return (int)voteType;
        }

        if (existing.VoteType == voteType)
            return 0;

        var delta = (int)voteType - (int)existing.VoteType;
        existing.ChangeVoteType(voteType);
        existing.MarkUpdated();
        unitOfWork.Votes.Update(existing);
        return delta;
    }
}
