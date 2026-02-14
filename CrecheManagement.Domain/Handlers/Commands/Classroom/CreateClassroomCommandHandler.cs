using System.Net;
using CrecheManagement.Domain.Commands.Class;
using CrecheManagement.Domain.Exceptions;
using CrecheManagement.Domain.Interfaces.Repositories;
using CrecheManagement.Domain.Interfaces.Services;
using CrecheManagement.Domain.Messages;
using CrecheManagement.Domain.Models;
using CrecheManagement.Domain.Responses.Class;
using CrecheManagement.Domain.Utils;
using MediatR;

namespace CrecheManagement.Domain.Handlers.Commands.Class;

public class CreateClassroomCommandHandler : IRequestHandler<CreateClassroomCommand, BaseResponse<CreatedClassromResponse>>
{
    private readonly IClassroomsRepository _classesRepository;
    private readonly ICrechesRepository _crechesRepository;
    private readonly ILoggedUser _loggedUser;

    public CreateClassroomCommandHandler(
        IClassroomsRepository classesRepository, 
        ICrechesRepository crechesRepository, 
        ILoggedUser loggedUser)
    {
        _classesRepository = classesRepository;
        _crechesRepository = crechesRepository;
        _loggedUser = loggedUser;
    }

    public async Task<BaseResponse<CreatedClassromResponse>> Handle(CreateClassroomCommand request, CancellationToken cancellationToken)
    {
        var user = await _loggedUser.GetUserAsync();
        var creche = await _crechesRepository.GetByIdentifierAsync(request.CrecheIdentifier!);

        if (creche == null || creche.UserIdentifier != user.Identifier)
            throw new CrecheManagementException(ReturnMessages.CRECHE_NOT_FOUND, HttpStatusCode.NotFound);

        var year = DateTime.Now.Year;

        if (await _classesRepository.ExistAsync(request.CrecheIdentifier!, request.Name, year))
            throw new CrecheManagementException(ReturnMessages.CLASSROOM_EXIST, HttpStatusCode.Conflict);

        var classRoom = new Models.Classroom
        {
            CrecheIdentifier = creche.Identifier,
            Name = request.Name,
            Year = year,
        };

        await _classesRepository.UpsertAsync(classRoom);

        return new BaseResponse<CreatedClassromResponse>
        {
            StatusCode = HttpStatusCode.Created,
            Message = ReturnMessages.CLASSROOM_CREATED_SUCCESSFULLY,
            Data = new CreatedClassromResponse
            {
                Name = classRoom.Name,
                Year = classRoom.Year
            }
        };
    }
}