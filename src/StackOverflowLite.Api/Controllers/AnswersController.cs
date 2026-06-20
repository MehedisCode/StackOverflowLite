using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StackOverflowLite.Application.Common.Models;
using StackOverflowLite.Application.Features.Answers.Commands.CreateAnswer;
using StackOverflowLite.Application.Features.Answers.Commands.DeleteAnswer;
using StackOverflowLite.Application.Features.Answers.Commands.UpdateAnswer;
using StackOverflowLite.Application.Features.Answers.DTOs;
using StackOverflowLite.Application.Features.Answers.Queries.GetAnswersByQuestion;

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
}
