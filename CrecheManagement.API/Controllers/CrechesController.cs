using CrecheManagement.API.Attributes;
using CrecheManagement.Domain.Commands.Creche;
using CrecheManagement.Domain.Queries.Creche;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace CrecheManagement.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CrechesController : ControllerBase
{
    private readonly IMediator _mediator;

    public CrechesController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    [IsAuthenticated]
    public async Task<IActionResult> Read()
    {
        var result = await _mediator.Send(new GetCrechesQuery());
        if (!result.Data.Any())
            return NoContent();

        return Created(string.Empty, result);
    }

    [HttpPost]
    [IsAuthenticated]
    public async Task<IActionResult> Create([FromBody] RegisterCrecheCommand command)
    {
        var result = await _mediator.Send(command);
        return Created(string.Empty, result);
    }

    [HttpPut]
    [IsAuthenticated]
    [Route("{identifier}")]
    public async Task<IActionResult> Update([FromRoute] string identifier, [FromBody] UpdateCrecheCommand command)
    {
        command.Identifier = identifier;

        await _mediator.Send(command);
        return NoContent();
    }

    [HttpDelete]
    [IsAuthenticated]
    [Route("{identifier}")]
    public async Task<IActionResult> Delete([FromRoute] string identifier)
    {
        await _mediator.Send(new DeleteCrecheCommand { Identifier = identifier });
        return NoContent();
    }
}