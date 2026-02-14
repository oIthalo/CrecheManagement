using System.Net;
using AutoMapper;
using CrecheManagement.Domain.Exceptions;
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
    private readonly ICrechesRepository _crechesRepository;
    private readonly ILoggedUser _loggedUser;
    private readonly IMapper _mapper;

    public GetClassroomsQueryHandler(
        IClassroomsRepository classroomsRepository, 
        ICrechesRepository crechesRepository, 
        ILoggedUser loggedUser, 
        IMapper mapper)
    {
        _classroomsRepository = classroomsRepository;
        _crechesRepository = crechesRepository;
        _loggedUser = loggedUser;
        _mapper = mapper;
    }

    public async Task<BaseResponse<List<ClassroomResponse>>> Handle(GetClassroomsQuery request, CancellationToken cancellationToken)
    {
        var user = await _loggedUser.GetUserAsync();
        var creche = await _crechesRepository.GetByIdentifierAsync(request.CrecheIdentifier!);

        if (creche == null || creche.UserIdentifier != user.Identifier)
            throw new CrecheManagementException(ReturnMessages.CRECHE_NOT_FOUND, HttpStatusCode.NotFound);

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