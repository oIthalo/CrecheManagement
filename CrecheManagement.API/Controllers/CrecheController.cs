using CrecheManagement.Domain.Commands.Creche;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace CrecheManagement.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CrecheController : ControllerBase
{
    private readonly IMediator _mediator;

    public CrecheController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] RegisterCrecheCommand command)
    {
        var result = await _mediator.Send(command);
        return Created(string.Empty, result);
    }
}