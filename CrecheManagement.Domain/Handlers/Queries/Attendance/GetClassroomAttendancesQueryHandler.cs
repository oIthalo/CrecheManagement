using System.Net;
using AutoMapper;
using CrecheManagement.Domain.Exceptions;
using CrecheManagement.Domain.Interfaces.Repositories;
using CrecheManagement.Domain.Interfaces.Services;
using CrecheManagement.Domain.Messages;
using CrecheManagement.Domain.Queries.Attendance;
using CrecheManagement.Domain.Responses.Attendance;
using CrecheManagement.Domain.Utils;
using MediatR;

namespace CrecheManagement.Domain.Handlers.Queries.Attendance;

public class GetClassroomAttendancesQueryHandler : IRequestHandler<GetClassroomAttendancesQuery, BaseResponse<List<AttendanceResponse>>>
{
    private readonly ICrechesRepository _crechesRepository;
    private readonly IAttendancesRepository _attendancesRepository;
    private readonly IClassroomsRepository _classroomsRepository;
    private readonly ILoggedUser _loggedUser;
    private readonly IMapper _mapper;

    public GetClassroomAttendancesQueryHandler(
        ICrechesRepository crechesRepository,
        IAttendancesRepository attendancesRepository,
        IClassroomsRepository classroomsRepository,
        ILoggedUser loggedUser,
        IMapper mapper)
    {
        _crechesRepository = crechesRepository;
        _attendancesRepository = attendancesRepository;
        _classroomsRepository = classroomsRepository;
        _loggedUser = loggedUser;
        _mapper = mapper;
    }

    public async Task<BaseResponse<List<AttendanceResponse>>> Handle(GetClassroomAttendancesQuery request, CancellationToken cancellationToken)
    {
        var user = await _loggedUser.GetUserAsync();
        var creche = await _crechesRepository.GetByIdentifierAsync(request.CrecheIdentifier!);

        if (creche == null || creche.UserIdentifier != user.Identifier)
            throw new CrecheManagementException(ReturnMessages.CRECHE_NOT_FOUND, HttpStatusCode.NotFound);

        var classroom = await _classroomsRepository.GetByIdentifierAsync(request.ClassroomIdentifier)
             ?? throw new CrecheManagementException(ReturnMessages.CLASSROOM_NOT_FOUND, HttpStatusCode.NotFound);

        var attendances = await _attendancesRepository.GetClassroomAttendancesAsync(classroom.Identifier, request.Date);

        return new BaseResponse<List<AttendanceResponse>>
        {
            Message = ReturnMessages.ATTENDANCES_RETURNED_SUCCESSFULLY,
            StatusCode = HttpStatusCode.OK,
            Data = _mapper.Map<List<AttendanceResponse>>(attendances)
        };
    }
}