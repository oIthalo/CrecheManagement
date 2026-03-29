using CrecheManagement.API.Attributes;
using CrecheManagement.Domain.Commands.Creche;
using CrecheManagement.Domain.Queries.Creche;
using CrecheManagement.Domain.Queries.Dashboard;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace CrecheManagement.API.Controllers;

[ApiController]
[IsAuthenticated]
[Route("api/[controller]")]
public class CrechesController : ControllerBase
{
    private readonly IMediator _mediator;

    public CrechesController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var result = await _mediator.Send(new GetCrechesQuery());
        if (!result.Data.Any())
            return NoContent();

        return Ok(result);
    }

    [HttpGet]
    [Route("{crecheIdentifier}")]
    public async Task<IActionResult> GetOne([FromRoute] string crecheIdentifier)
    {
        var result = await _mediator.Send(new GetCrecheQuery { Identifier = crecheIdentifier });
        return Ok(result);
    }

    [HttpGet]
    [Route("{crecheIdentifier}/dashboard")]
    public async Task<IActionResult> GetDashboard(string crecheIdentifier)
    {
        var result = await _mediator.Send(new GetDashboardQuery { CrecheIdentifier = crecheIdentifier });
        return Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] RegisterCrecheCommand command)
    {
        var result = await _mediator.Send(command);
        return Created(string.Empty, result);
    }

    [HttpPatch]
    [Route("{crecheIdentifier}")]
    public async Task<IActionResult> Update([FromRoute] string crecheIdentifier, [FromBody] UpdateCrecheCommand command)
    {
        command.Identifier = crecheIdentifier;

        await _mediator.Send(command);
        return NoContent();
    }

    [HttpDelete]
    [Route("{crecheIdentifier}")]
    public async Task<IActionResult> Delete([FromRoute] string crecheIdentifier)
    {
        await _mediator.Send(new DeleteCrecheCommand { Identifier = crecheIdentifier });
        return NoContent();
    }
}