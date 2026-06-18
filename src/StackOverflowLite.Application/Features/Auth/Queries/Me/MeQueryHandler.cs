using MediatR;
using StackOverflowLite.Application.Common.Interfaces;
using StackOverflowLite.Application.Features.Auth.DTOs;

namespace StackOverflowLite.Application.Features.Auth.Queries.Me;

public class MeQueryHandler(
    ICurrentUser currentUser,
    IIdentityService identity) : IRequestHandler<MeQuery, CurrentUserDto?>
{
    public async Task<CurrentUserDto?> Handle(MeQuery request, CancellationToken cancellationToken)
    {
        if (!currentUser.IsAuthenticated || currentUser.Id is null)
        {
            return null;
        }

        return await identity.GetByIdAsync(currentUser.Id.Value, cancellationToken);
    }
}
