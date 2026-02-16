using System.Net;
using CrecheManagement.Domain.Commands.Class;
using CrecheManagement.Domain.Exceptions;
using CrecheManagement.Domain.Interfaces.Repositories;
using CrecheManagement.Domain.Interfaces.Services;
using CrecheManagement.Domain.Messages;
using CrecheManagement.Domain.Responses.Class;
using CrecheManagement.Domain.Utils;
using MediatR;

namespace CrecheManagement.Domain.Handlers.Commands.Class;

public class CreateClassroomCommandHandler : IRequestHandler<CreateClassroomCommand, BaseResponse<CreatedClassromResponse>>
{
    private readonly IClassroomsRepository _classesRepository;
    private readonly ICrecheAuthorizationService _crecheAuthorizationService;

    public CreateClassroomCommandHandler(
        IClassroomsRepository classesRepository,
        ICrecheAuthorizationService crecheAuthorizationService)
    {
        _classesRepository = classesRepository;
        _crecheAuthorizationService = crecheAuthorizationService;
    }

    public async Task<BaseResponse<CreatedClassromResponse>> Handle(CreateClassroomCommand request, CancellationToken cancellationToken)
    {
        var creche = await _crecheAuthorizationService.AuthorizeAndGetCreche(request.CrecheIdentifier!);

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
                Identifier = classRoom.Identifier,
                Name = classRoom.Name,
                Year = classRoom.Year
            }
        };
    }
}