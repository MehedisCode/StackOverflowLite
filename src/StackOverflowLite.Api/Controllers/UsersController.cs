using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StackOverflowLite.Application.Features.Users.DTOs;
using StackOverflowLite.Application.Features.Users.Queries.GetUserStats;

namespace StackOverflowLite.Api.Controllers;

[ApiController]
[Route("api/users")]
public class UsersController(ISender sender) : ControllerBase
{
    [HttpGet("{userId:guid}/stats")]
    [AllowAnonymous]
    public async Task<ActionResult<UserStatsDto>> GetStats(
        Guid userId, CancellationToken cancellationToken)
    {
        var dto = await sender.Send(new GetUserStatsQuery(userId), cancellationToken);
        return dto is null ? NotFound() : Ok(dto);
    }
}
