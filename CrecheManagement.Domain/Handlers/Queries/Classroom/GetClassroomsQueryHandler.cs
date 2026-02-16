using System.Net;
using AutoMapper;
using CrecheManagement.Domain.Interfaces.Repositories;
using CrecheManagement.Domain.Interfaces.Services;
using CrecheManagement.Domain.Messages;
using CrecheManagement.Domain.Queries.Classroom;
using CrecheManagement.Domain.Responses.Classroom;
using CrecheManagement.Domain.Utils;
using MediatR;

namespace CrecheManagement.Domain.Handlers.Queries.Classroom;

public class GetClassroomsQueryHandler : IRequestHandler<GetClassroomsQuery, BaseResponse<List<ClassroomResponse>>>
{
    private readonly IClassroomsRepository _classroomsRepository;
    private readonly ICrecheAuthorizationService _crecheAuthorizationService;
    private readonly IMapper _mapper;

    public GetClassroomsQueryHandler(
        IClassroomsRepository classroomsRepository,
        ICrecheAuthorizationService crecheAuthorizationService,
        IMapper mapper)
    {
        _classroomsRepository = classroomsRepository;
        _crecheAuthorizationService = crecheAuthorizationService;
        _mapper = mapper;
    }

    public async Task<BaseResponse<List<ClassroomResponse>>> Handle(GetClassroomsQuery request, CancellationToken cancellationToken)
    {
        var creche = await _crecheAuthorizationService.AuthorizeAndGetCreche(request.CrecheIdentifier!);

        var year = request.Year ?? DateTime.Now.Year;
        var classrooms = await _classroomsRepository.GetClassroomsAsync(request.CrecheIdentifier!, year);

        return new BaseResponse<List<ClassroomResponse>>
        {
            StatusCode = HttpStatusCode.OK,
            Message = ReturnMessages.CLASSROOMS_RETURNED_SUCCESSFULLY,
            Data = _mapper.Map<List<ClassroomResponse>>(classrooms),
        };
    }
}