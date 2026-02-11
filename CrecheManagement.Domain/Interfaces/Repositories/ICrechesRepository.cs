using CrecheManagement.Domain.Models;

namespace CrecheManagement.Domain.Interfaces.Repositories;

public interface ICrechesRepository
{
    Task<List<Creche>> GetAllAsync(string userIdentifier);
    Task<Creche?> GetByIdentifierAsync(string identifier);
    Task<bool> ExistWithCNPJAsync(string cnpj);
    Task<bool> ExistWithEmailAsync(string email);
    Task UpsertAsync(Creche creche);
    Task DeleteAsync(Creche creche);
}