using CrecheManagement.Domain.Models;

namespace CrecheManagement.Domain.Interfaces.Security;

public interface ILoggedUser
{
    Task<User> GetUserAsync();
}