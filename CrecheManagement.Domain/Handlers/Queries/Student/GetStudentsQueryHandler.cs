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
    private readonly IClassroomsRepository _classroomsRepository;
    private readonly ICrecheAuthorizationService _crecheAuthorizationService;
    private readonly IMapper _mapper;

    public GetStudentsQueryHandler(
        IStudentsRepository studentsRepository,
        IClassroomsRepository classroomsRepository,
        ICrecheAuthorizationService crecheAuthorizationService,
        IMapper mapper)
    {
        _studentsRepository = studentsRepository;
        _classroomsRepository = classroomsRepository;
        _crecheAuthorizationService = crecheAuthorizationService;
        _mapper = mapper;
    }

    public async Task<BaseResponse<List<StudentResponse>>> Handle(GetStudentsQuery request, CancellationToken cancellationToken)
    {
        var creche = await _crecheAuthorizationService.AuthorizeAndGetCreche(request.CrecheIdentifier!);

        var students = new List<Models.Student>();

        if (string.IsNullOrEmpty(request.ClassroomIdentifier))
        {
            students = await _studentsRepository.GetStudentsByCrecheAsync(request.CrecheIdentifier);
        }
        else
        {
            var classroom = await _classroomsRepository.GetByIdentifierAsync(request.ClassroomIdentifier)
                ?? throw new CrecheManagementException(ReturnMessages.CLASSROOM_NOT_FOUND, HttpStatusCode.NotFound);

            students = await _studentsRepository.GetStudentsByClassroomAsync(request.ClassroomIdentifier);
        }

        return new BaseResponse<List<StudentResponse>>
        {
            StatusCode = HttpStatusCode.OK,
            Message = ReturnMessages.STUDENTS_RETURNED_SUCCESSFULLY,
            Data = _mapper.Map<List<StudentResponse>>(students)
        };
    }
}