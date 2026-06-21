using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using StackOverflowLite.Application.Common.Models;
using StackOverflowLite.Application.Features.Answers.DTOs;
using StackOverflowLite.Application.Features.Answers.Commands.CreateAnswer;
using StackOverflowLite.Application.Features.Answers.Commands.AcceptAnswer;
using StackOverflowLite.Application.Features.Answers.Commands.DeleteAnswer;
using StackOverflowLite.Application.Features.Answers.Commands.UnacceptAnswer;
using StackOverflowLite.Application.Features.Answers.Commands.UpdateAnswer;
using StackOverflowLite.Application.Features.Answers.Queries.GetAnswersByQuestion;
using StackOverflowLite.Application.Features.Votes.Commands.DownvoteAnswer;
using StackOverflowLite.Application.Features.Votes.Commands.RemoveAnswerVote;
using StackOverflowLite.Application.Features.Votes.Commands.UpvoteAnswer;

namespace StackOverflowLite.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AnswersController(ISender sender) : ControllerBase
{
    [HttpPost]
    [Authorize]
    public async Task<ActionResult<AnswerDto>> Create(
        [FromBody] CreateAnswerRequest request,
        CancellationToken cancellationToken)
    {
        var dto = await sender.Send(
            new CreateAnswerCommand(request.QuestionId, request.Body),
            cancellationToken);
        return Ok(dto);
    }

    [HttpGet]
    [AllowAnonymous]
    public async Task<ActionResult<PagedResult<AnswerListItemDto>>> List(
        [FromQuery] Guid questionId,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 20,
        CancellationToken cancellationToken = default)
    {
        var result = await sender.Send(
            new GetAnswersByQuestionQuery(questionId, page, pageSize),
            cancellationToken);
        return Ok(result);
    }

    [HttpPut("{id:guid}")]
    [Authorize]
    public async Task<ActionResult<AnswerDto>> Update(
        Guid id,
        [FromBody] UpdateAnswerRequest request,
        CancellationToken cancellationToken)
    {
        var dto = await sender.Send(
            new UpdateAnswerCommand(id, request.Body),
            cancellationToken);
        return Ok(dto);
    }

    [HttpDelete("{id:guid}")]
    [Authorize]
    public async Task<IActionResult> Delete(
        Guid id,
        CancellationToken cancellationToken)
    {
        await sender.Send(new DeleteAnswerCommand(id), cancellationToken);
        return NoContent();
    }

    [HttpPost("{id:guid}/accept")]
    [Authorize]
    public async Task<IActionResult> Accept(
        Guid id,
        CancellationToken cancellationToken)
    {
        await sender.Send(new AcceptAnswerCommand(id), cancellationToken);
        return NoContent();
    }

    [HttpDelete("{id:guid}/accept")]
    [Authorize]
    public async Task<IActionResult> Unaccept(
        Guid id,
        CancellationToken cancellationToken)
    {
        await sender.Send(new UnacceptAnswerCommand(id), cancellationToken);
        return NoContent();
    }

    [HttpPost("{id:guid}/upvote")]
    [Authorize]
    public async Task<IActionResult> Upvote(
        Guid id,
        CancellationToken cancellationToken)
    {
        await sender.Send(new UpvoteAnswerCommand(id), cancellationToken);
        return NoContent();
    }

    [HttpPost("{id:guid}/downvote")]
    [Authorize]
    public async Task<IActionResult> Downvote(
        Guid id,
        CancellationToken cancellationToken)
    {
        await sender.Send(new DownvoteAnswerCommand(id), cancellationToken);
        return NoContent();
    }

    [HttpDelete("{id:guid}/vote")]
    [Authorize]
    public async Task<IActionResult> RemoveVote(
        Guid id,
        CancellationToken cancellationToken)
    {
        await sender.Send(new RemoveAnswerVoteCommand(id), cancellationToken);
        return NoContent();
    }
}
