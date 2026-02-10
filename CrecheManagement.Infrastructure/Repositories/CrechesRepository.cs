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

    public async Task AddAsync(Creche creche)
    {
        await _mongo.Creches.InsertOneAsync(creche);
    }

    public async Task<bool> ExistAsync(string cnpj)
    {
        var filter = Builders<Creche>.Filter.Eq(x => x.CNPJ, cnpj);
        return await _mongo.Creches.Find(filter).AnyAsync();
    }
}