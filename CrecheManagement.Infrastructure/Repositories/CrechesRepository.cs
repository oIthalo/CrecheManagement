using CrecheManagement.Domain.Interfaces.Repositories;
using CrecheManagement.Domain.Models;
using CrecheManagement.Infrastructure.Context;
using MongoDB.Driver;

namespace CrecheManagement.Infrastructure.Repositories;

public class CrechesRepository : ICrechesRepository
{
    private readonly MongoContext _mongo;

    public CrechesRepository(MongoContext mongoDb)
    {
        _mongo = mongoDb;
    }

    public async Task<List<Creche>> GetAllAsync(string userIdentifier)
    {
        var filter = Builders<Creche>.Filter.Eq(x => x.UserIdentifier, userIdentifier);
        return await _mongo.Creches.Find(filter).ToListAsync();
    }

    public async Task<Creche?> GetByIdentifierAsync(string identifier)
    {
        var filter = Builders<Creche>.Filter.Eq(x => x.Identifier, identifier);
        return await _mongo.Creches.Find(filter).FirstOrDefaultAsync();
    }

    public async Task<bool> ExistWithCNPJAsync(string cnpj)
    {
        var filter = Builders<Creche>.Filter.Eq(x => x.CNPJ, cnpj);
        return await _mongo.Creches.Find(filter).AnyAsync();
    }

    public async Task<bool> ExistWithEmailAsync(string email)
    {
        var filter = Builders<Creche>.Filter.Eq(x => x.Email, email);
        return await _mongo.Creches.Find(filter).AnyAsync();
    }

    public async Task UpsertAsync(Creche creche)
    {
        var filter = Builders<Creche>.Filter.Eq(x => x.Identifier, creche.Identifier);
        await _mongo.Creches.ReplaceOneAsync(filter, creche, new ReplaceOptions { IsUpsert = true });
    }

    public async Task DeleteAsync(Creche creche)
    {
        await _mongo.Creches.DeleteOneAsync(x => x.Identifier == creche.Identifier);
    }
}