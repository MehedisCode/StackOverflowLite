using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StackOverflowLite.Application.Common.Models;
using StackOverflowLite.Application.Features.Questions.Commands.CreateQuestion;
using StackOverflowLite.Application.Features.Questions.Commands.DeleteQuestion;
using StackOverflowLite.Application.Features.Questions.Commands.UpdateQuestion;
using StackOverflowLite.Application.Features.Questions.DTOs;
using StackOverflowLite.Application.Features.Questions.Queries.GetQuestionById;
using StackOverflowLite.Application.Features.Questions.Queries.GetQuestions;

namespace StackOverflowLite.Api.Controllers;

[ApiController]
[Route("api/questions")]
public class QuestionsController(ISender sender) : ControllerBase
{
    [HttpPost]
    [Authorize]
    public async Task<ActionResult<QuestionDto>> Create(
        [FromBody] CreateQuestionRequest request,
        CancellationToken cancellationToken)
    {
        var dto = await sender.Send(
            new CreateQuestionCommand(
                request.Title,
                request.Body,
                request.Tags),
            cancellationToken);
        return Ok(dto);
    }

    [HttpPut("{id:guid}")]
    [Authorize]
    public async Task<ActionResult<QuestionDto>> Update(
        Guid id,
        [FromBody] UpdateQuestionRequest request,
        CancellationToken cancellationToken)
    {
        var dto = await sender.Send(
            new UpdateQuestionCommand(
                id,
                request.Title,
                request.Body,
                request.Tags),
            cancellationToken);
        return Ok(dto);
    }

    [HttpDelete("{id:guid}")]
    [Authorize]
    public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken)
    {
        await sender.Send(new DeleteQuestionCommand(id), cancellationToken);
        return NoContent();
    }

    [HttpGet("{id:guid}")]
    [AllowAnonymous]
    public async Task<ActionResult<QuestionDto>> GetById(
        Guid id,
        CancellationToken cancellationToken)
    {
        var dto = await sender.Send(new GetQuestionByIdQuery(id), cancellationToken);
        return dto is null ? NotFound() : Ok(dto);
    }

    [HttpGet]
    [AllowAnonymous]
    public async Task<ActionResult<PagedResult<QuestionListItemDto>>> List(
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 20,
        [FromQuery] string? tag = null,
        [FromQuery] string? search = null,
        CancellationToken cancellationToken = default)
    {
        var result = await sender.Send(
            new GetQuestionsQuery(page, pageSize, tag, search),
            cancellationToken);
        return Ok(result);
    }
}
