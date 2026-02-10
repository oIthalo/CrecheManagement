using CrecheManagement.Domain.Models;

namespace CrecheManagement.Domain.Interfaces.Services;

public interface ILoggedUserService
{
    Task<User> GetUser();
}