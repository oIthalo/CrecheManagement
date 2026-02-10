using CrecheManagement.Domain.Models;

namespace CrecheManagement.Domain.Interfaces.Repositories;

public interface ICrechesRepository
{
    Task AddAsync(Creche creche);
    Task<bool> ExistAsync(string cnpj);
}