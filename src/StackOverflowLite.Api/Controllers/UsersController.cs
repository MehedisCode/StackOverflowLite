using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StackOverflowLite.Application.Features.Users.DTOs;
using StackOverflowLite.Application.Features.Users.Queries.GetUserProfile;

namespace StackOverflowLite.Api.Controllers;

[ApiController]
[Route("api/users")]
public class UsersController(ISender sender) : ControllerBase
{
    [HttpGet("me/profile")]
    [Authorize]
    public async Task<ActionResult<UserProfileDto>> GetProfile(CancellationToken cancellationToken)
    {
        var dto = await sender.Send(new GetUserProfileQuery(), cancellationToken);
        return dto is null ? NotFound() : Ok(dto);
    }
}
