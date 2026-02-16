using CrecheManagement.API.Attributes;
using CrecheManagement.Domain.Commands.Attendance;
using CrecheManagement.Domain.Queries.Attendance;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace CrecheManagement.API.Controllers;

[ApiController]
[IsAuthenticated]
[Route("api/creches/{crecheIdentifier}/classrooms/{classroomIdentifier}/attendances")]
public class AttendancesController : ControllerBase
{
    private readonly IMediator _mediator;

    public AttendancesController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<IActionResult> Read(
        [FromRoute] string crecheIdentifier,
        [FromRoute] string classroomIdentifier,
        [FromQuery] DateTime? date)
    {
        var result = await _mediator.Send(new GetClassroomAttendancesQuery
        {
            CrecheIdentifier = crecheIdentifier,
            ClassroomIdentifier = classroomIdentifier,
            Date = date
        });

        if (result.Data.Count == 0)
            return NoContent();

        return Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> Create(
        [FromRoute] string crecheIdentifier,
        [FromRoute] string classroomIdentifier,
        [FromBody] RegisterAttendanceCommand command)
    {
        command.CrecheIdentifier = crecheIdentifier;
        command.ClassroomIdentifier = classroomIdentifier;

        var result = await _mediator.Send(command);
        return Ok(result);
    }
}