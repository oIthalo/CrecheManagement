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
    private readonly IAttendancesRepository _attendancesRepository;
    private readonly IClassroomsRepository _classroomsRepository;
    private readonly ICrecheAuthorizationService _crecheAuthorizationService;
    private readonly IMapper _mapper;

    public GetClassroomAttendancesQueryHandler(
        IAttendancesRepository attendancesRepository,
        IClassroomsRepository classroomsRepository,
        ICrecheAuthorizationService crecheAuthorizationService,
        IMapper mapper)
    {
        _attendancesRepository = attendancesRepository;
        _classroomsRepository = classroomsRepository;
        _crecheAuthorizationService = crecheAuthorizationService;
        _mapper = mapper;
    }

    public async Task<BaseResponse<List<AttendanceResponse>>> Handle(GetClassroomAttendancesQuery request, CancellationToken cancellationToken)
    {
        var creche = await _crecheAuthorizationService.AuthorizeAndGetCreche(request.CrecheIdentifier!);

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