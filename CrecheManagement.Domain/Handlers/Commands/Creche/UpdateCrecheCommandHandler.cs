using System.Net;
using CrecheManagement.Domain.Commands.Creche;
using CrecheManagement.Domain.Exceptions;
using CrecheManagement.Domain.Interfaces.Repositories;
using CrecheManagement.Domain.Interfaces.Services;
using CrecheManagement.Domain.Messages;
using MediatR;

namespace CrecheManagement.Domain.Handlers.Commands.Creche;

public class UpdateCrecheCommandHandler : IRequestHandler<UpdateCrecheCommand>
{
    private readonly ICrechesRepository _crechesRepository;
    private readonly ILoggedUser _loggedUser;

    public UpdateCrecheCommandHandler(ICrechesRepository crechesRepository, ILoggedUser loggedUser)
    {
        _crechesRepository = crechesRepository;
        _loggedUser = loggedUser;
    }

    public async Task Handle(UpdateCrecheCommand request, CancellationToken cancellationToken)
    {
        var user = await _loggedUser.GetUserAsync();
        var creche = await _crechesRepository.GetByIdentifierAsync(request.Identifier!);

        if (creche == null || creche.UserIdentifier != user.Identifier)
            throw new CrecheManagementException(ReturnMessages.CRECHE_NOT_FOUND, HttpStatusCode.NotFound);

        creche.Name = !string.IsNullOrEmpty(request.Name) ? request.Name : creche.Name;
        creche.Email = !string.IsNullOrEmpty(request.Email) ? request.Email : creche.Email;
        creche.ContactNumber = !string.IsNullOrEmpty(request.ContactNumber) ? request.ContactNumber : creche.ContactNumber;
        creche.Address = request.Address ?? creche.Address;

        await _crechesRepository.UpsertAsync(creche);
    }
}