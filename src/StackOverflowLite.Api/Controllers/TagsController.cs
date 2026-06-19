using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StackOverflowLite.Application.Common.Models;
using StackOverflowLite.Application.Features.Questions.DTOs;
using StackOverflowLite.Application.Features.Tags.Commands.AssignTagToQuestion;
using StackOverflowLite.Application.Features.Tags.Commands.CreateTag;
using StackOverflowLite.Application.Features.Tags.Commands.RemoveTagFromQuestion;
using StackOverflowLite.Application.Features.Tags.DTOs;
using StackOverflowLite.Application.Features.Tags.Queries.GetQuestionsByTag;
using StackOverflowLite.Application.Features.Tags.Queries.GetTags;

namespace StackOverflowLite.Api.Controllers;

[ApiController]
[Route("api/tags")]
public class TagsController(ISender sender) : ControllerBase
{
    [HttpPost]
    [Authorize]
    public async Task<ActionResult<TagDto>> Create(
        [FromBody] CreateTagRequest request,
        CancellationToken cancellationToken)
    {
        var dto = await sender.Send(
            new CreateTagCommand(request.Name, request.Description),
            cancellationToken);

        return CreatedAtAction(
            nameof(GetQuestionsByTag),
            new { tagName = dto.Name },
            dto);
    }

    [HttpGet]
    [AllowAnonymous]
    public async Task<ActionResult<PagedResult<TagListItemDto>>> List(
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 20,
        [FromQuery] string? search = null,
        CancellationToken cancellationToken = default)
    {
        var result = await sender.Send(
            new GetTagsQuery(page, pageSize, search),
            cancellationToken);
        return Ok(result);
    }

    [HttpPost("assignments")]
    [Authorize]
    public async Task<IActionResult> Assign(
        [FromBody] AssignTagRequest request,
        CancellationToken cancellationToken)
    {
        await sender.Send(
            new AssignTagToQuestionCommand(request.QuestionId, request.TagName),
            cancellationToken);
        return NoContent();
    }

    [HttpDelete("assignments")]
    [Authorize]
    public async Task<IActionResult> Unassign(
        [FromBody] AssignTagRequest request,
        CancellationToken cancellationToken)
    {
        await sender.Send(
            new RemoveTagFromQuestionCommand(request.QuestionId, request.TagName),
            cancellationToken);
        return NoContent();
    }

    [HttpGet("{tagName}/questions")]
    [AllowAnonymous]
    public async Task<ActionResult<PagedResult<QuestionListItemDto>>> GetQuestionsByTag(
        string tagName,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 20,
        CancellationToken cancellationToken = default)
    {
        var result = await sender.Send(
            new GetQuestionsByTagQuery(tagName, page, pageSize),
            cancellationToken);
        return Ok(result);
    }
}
