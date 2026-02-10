using CrecheManagement.Domain.Models;

namespace CrecheManagement.Domain.Interfaces.Repositories;

public interface IUsersRepository
{
    Task<User?> GetByIdentifierAsync(string identifier);
    Task<User?> GetByEmailAsync(string email);
    Task<bool> ExistWithEmailAsync(string email);
    Task UpsertAsync(User user);
}