using CrecheManagement.Domain.Models;

namespace CrecheManagement.Domain.Interfaces.Services;

public interface ILoggedUser
{
    Task<User> GetUserAsync();
}