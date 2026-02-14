using AutoMapper;
using CrecheManagement.API.Attributes;
using CrecheManagement.Domain.Commands.Student;
using CrecheManagement.Domain.Queries.Student;
using CrecheManagement.Domain.Requests;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace CrecheManagement.API.Controllers;

[ApiController]
[IsAuthenticated]
[Route("api/creches/{crecheIdentifier}")]
public class StudentsController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly IMapper _mapper;

    public StudentsController(IMediator mediator, IMapper mapper)
    {
        _mediator = mediator;
        _mapper = mapper;
    }

    [HttpGet]
    [Route("students")]
    public async Task<IActionResult> GetAll([FromRoute] string crecheIdentifier)
    {
        var result = await _mediator.Send(new GetStudentsQuery { CrecheIdentifier = crecheIdentifier });

        if (result.Data.Count == 0)
            return NoContent();

        return Ok(result);
    }

    [HttpGet]
    [Route("classrooms/{classroomIdentifier}/students")]
    public async Task<IActionResult> GetAll([FromRoute] string crecheIdentifier, [FromRoute] string classroomIdentifier)
    {
        var result = await _mediator.Send(new GetStudentsQuery { CrecheIdentifier = crecheIdentifier, ClassroomIdentifier = classroomIdentifier });

        if (result.Data.Count == 0)
            return NoContent();

        return Ok(result);
    }

    [HttpPost]
    [Route("students")]
    public async Task<IActionResult> Create([FromRoute] string crecheIdentifier, [FromForm] RegisterStudentRequest request)
    {
        var command = _mapper.Map<RegisterStudentCommand>(request);
        command.CrecheIdentifier = crecheIdentifier;

        var result = await _mediator.Send(command);
        return Created(string.Empty, result);
    }

    [HttpPatch]
    [Route("students/{studentIdentifier}")]
    public async Task<IActionResult> Create(
        [FromRoute] string crecheIdentifier,
        [FromRoute] string studentIdentifier,
        [FromBody] UpdateStudentCommand command)
    {
        command.CrecheIdentifier = crecheIdentifier;
        command.StudentIdentifier = studentIdentifier;

        await _mediator.Send(command);
        return NoContent();
    }
}