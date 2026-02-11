using System.Net;
using CrecheManagement.Domain.Commands.Creche;
using CrecheManagement.Domain.Exceptions;
using CrecheManagement.Domain.Interfaces.Repositories;
using CrecheManagement.Domain.Interfaces.Services;
using CrecheManagement.Domain.Messages;
using MediatR;

namespace CrecheManagement.Domain.Handlers.Commands.Creche;

public class DeleteCrecheCommandHandler : IRequestHandler<DeleteCrecheCommand>
{
    private readonly ICrechesRepository _crechesRepository;
    private readonly ILoggedUser _loggedUser;

    public DeleteCrecheCommandHandler(ICrechesRepository crechesRepository, ILoggedUser loggedUser)
    {
        _crechesRepository = crechesRepository;
        _loggedUser = loggedUser;
    }

    public async Task Handle(DeleteCrecheCommand request, CancellationToken cancellationToken)
    {
        var user = await _loggedUser.GetUserAsync();

        var creche = await _crechesRepository.GetByIdentifierAsync(request.Identifier!)
            ?? throw new CrecheManagementException(ReturnMessages.CRECHE_NOT_FOUND, HttpStatusCode.NotFound);

        if (creche.UserIdentifier != user.Identifier)
            throw new CrecheManagementException(ReturnMessages.CRECHE_NOT_FOUND, HttpStatusCode.NotFound);

        await _crechesRepository.DeleteAsync(creche);
    }
}