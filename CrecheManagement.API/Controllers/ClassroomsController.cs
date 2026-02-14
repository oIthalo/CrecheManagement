using CrecheManagement.API.Attributes;
using CrecheManagement.Domain.Commands.Class;
using CrecheManagement.Domain.Commands.Classroom;
using CrecheManagement.Domain.Queries.Classroom;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace CrecheManagement.API.Controllers;

[ApiController]
[IsAuthenticated]
[Route("api/creches/{crecheIdentifier}/classrooms")]
public class ClassroomsController : ControllerBase
{
    private readonly IMediator _mediator;

    public ClassroomsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<IActionResult> Read([FromRoute] string crecheIdentifier, [FromQuery] int? year)
    {
        var result = await _mediator.Send(new GetClassroomsQuery { CrecheIdentifier = crecheIdentifier, Year = year });

        if (result.Data.Count == 0)
            return NoContent();

        return Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromRoute] string crecheIdentifier, [FromBody] CreateClassroomCommand command)
    {
        command.CrecheIdentifier = crecheIdentifier;

        var result = await _mediator.Send(command);
        return Created(string.Empty, result);
    }

    [HttpPost]
    [Route("{classroomIdentifier}/students")]
    public async Task<IActionResult> InsertStudent(
        [FromRoute] string crecheIdentifier,
        [FromRoute] string classroomIdentifier,
        [FromBody] InsertStudentsToClassroomCommand command)
    {
        command.CrecheIdentifier = crecheIdentifier;
        command.ClassroomIdentifier = classroomIdentifier;

        var result = await _mediator.Send(command);
        return Ok(result);
    }

    [HttpDelete]
    [Route("{classroomIdentifier}")]
    public async Task<IActionResult> Delete(
        [FromRoute] string crecheIdentifier,
        [FromRoute] string classroomIdentifier)
    {
        await _mediator.Send(new DeleteClassroomCommand { ClassroomIdentifier = classroomIdentifier, CrecheIdentifier = crecheIdentifier });
        return NoContent();
    }
}