using System.Net;
using CrecheManagement.Domain.Exceptions;
using CrecheManagement.Domain.Interfaces.Repositories;
using CrecheManagement.Domain.Interfaces.Security;
using CrecheManagement.Domain.Interfaces.Services;
using CrecheManagement.Domain.Messages;
using CrecheManagement.Domain.Models;

namespace CrecheManagement.Infrastructure.Services;

public class CrecheAuthorizationService : ICrecheAuthorizationService
{
    private readonly ICrechesRepository _crechesRepository;
    private readonly ILoggedUser _loggedUser;

    public CrecheAuthorizationService(
        ICrechesRepository crechesRepository, 
        ILoggedUser loggedUser)
    {
        _crechesRepository = crechesRepository;
        _loggedUser = loggedUser;
    }

    public async Task<Creche> AuthorizeAndGetCreche(string crecheIdentifier)
    {
        var user = await _loggedUser.GetUserAsync();
        var creche = await _crechesRepository.GetByIdentifierAsync(crecheIdentifier);

        if (creche == null || creche.UserIdentifier != user.Identifier)
            throw new CrecheManagementException(ReturnMessages.CRECHE_NOT_FOUND, HttpStatusCode.NotFound);

        return creche;
    }
}