using MediatR;
using StackOverflowLite.Application.Common.Exceptions;
using StackOverflowLite.Application.Common.Interfaces;
using StackOverflowLite.Application.Features.Tags.Common;
using StackOverflowLite.Application.Features.Tags.DTOs;
using StackOverflowLite.Domain.Entities;

namespace StackOverflowLite.Application.Features.Tags.Commands.CreateTag;

public class CreateTagCommandHandler(
    ICurrentUser currentUser,
    IUnitOfWork unitOfWork) : IRequestHandler<CreateTagCommand, TagDto>
{
    public async Task<TagDto> Handle(
        CreateTagCommand request, CancellationToken cancellationToken)
    {
        if (currentUser.Id is null)
        {
            throw new ForbiddenAccessException("User is not authenticated.");
        }

        var name = TagMapping.NormalizeName(request.Name);

        var existing = await unitOfWork.Tags.GetByNameAsync(name, cancellationToken);
        if (existing is not null)
        {
            throw new DuplicateTagException(name);
        }

        var tag = new Tag
        {
            Name = name,
            Description = request.Description?.Trim()
        };

        await unitOfWork.Tags.AddAsync(tag, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return TagMapping.ToDto(tag);
    }
}
