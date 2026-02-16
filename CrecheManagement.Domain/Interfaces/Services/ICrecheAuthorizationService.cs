using CrecheManagement.Domain.Models;

namespace CrecheManagement.Domain.Interfaces.Services;

public interface ICrecheAuthorizationService
{
    Task<Creche> AuthorizeAndGetCreche(string crecheIdentifier);
}