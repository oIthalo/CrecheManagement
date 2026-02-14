using System.Net;
using AutoMapper;
using CrecheManagement.Domain.Exceptions;
using CrecheManagement.Domain.Interfaces.Repositories;
using CrecheManagement.Domain.Interfaces.Services;
using CrecheManagement.Domain.Messages;
using CrecheManagement.Domain.Queries.Student;
using CrecheManagement.Domain.Responses.Student;
using CrecheManagement.Domain.Utils;
using MediatR;

namespace CrecheManagement.Domain.Handlers.Queries.Student;

public class GetStudentsQueryHandler : IRequestHandler<GetStudentsQuery, BaseResponse<List<StudentResponse>>>
{
    private readonly IStudentsRepository _studentsRepository;
    private readonly ICrechesRepository _crechesRepository;
    private readonly IClassroomsRepository _classroomsRepository;
    private readonly ILoggedUser _loggedUser;
    private readonly IMapper _mapper;

    public GetStudentsQueryHandler(
        IStudentsRepository studentsRepository,
        ICrechesRepository crechesRepository,
        IClassroomsRepository classroomsRepository,
        ILoggedUser loggedUser,
        IMapper mapper)
    {
        _studentsRepository = studentsRepository;
        _crechesRepository = crechesRepository;
        _classroomsRepository = classroomsRepository;
        _loggedUser = loggedUser;
        _mapper = mapper;
    }

    public async Task<BaseResponse<List<StudentResponse>>> Handle(GetStudentsQuery request, CancellationToken cancellationToken)
    {
        var user = await _loggedUser.GetUserAsync();
        var creche = await _crechesRepository.GetByIdentifierAsync(request.CrecheIdentifier);

        if (creche == null || creche.UserIdentifier != user.Identifier)
            throw new CrecheManagementException(ReturnMessages.CRECHE_NOT_FOUND, HttpStatusCode.NotFound);

        var students = new List<Models.Student>();

        if (string.IsNullOrEmpty(request.ClassroomIdentifier)) 
            students = await _studentsRepository.GetStudentsByCrecheAsync(request.CrecheIdentifier);
        else 
            students = await _studentsRepository.GetStudentsByClassroomAsync(request.ClassroomIdentifier);

        return new BaseResponse<List<StudentResponse>>
        {
            StatusCode = HttpStatusCode.OK,
            Message = ReturnMessages.STUDENTS_RETURNED_SUCCESSFULLY,
            Data = _mapper.Map<List<StudentResponse>>(students)
        };
    }
}