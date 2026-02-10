using CrecheManagement.Domain.Interfaces.Repositories;
using CrecheManagement.Domain.Models;
using CrecheManagement.Infrastructure.Context;
using MongoDB.Driver;

namespace CrecheManagement.Infrastructure.Repositories;

public class UsersRepository : IUsersRepository
{
    private readonly MongoContext _mongo;

    public UsersRepository(MongoContext mongo)
    {
        _mongo = mongo;
    }

    public async Task<User?> GetByIdentifierAsync(string identifier)
    {
        var filter = Builders<User>.Filter.Eq(x => x.Identifier, identifier);
        return await _mongo.Users.Find(filter).FirstOrDefaultAsync();
    }

    public async Task<User?> GetByEmailAsync(string email)
    {
        var filter = Builders<User>.Filter.Eq(x => x.Email, email);
        return await _mongo.Users.Find(filter).FirstOrDefaultAsync();
    }

    public async Task<User?> GetByRefreshTokenAsync(string refreshToken)
    {
        var filter = Builders<User>.Filter.Eq(x => x.RefreshToken, refreshToken);
        return await _mongo.Users.Find(filter).FirstOrDefaultAsync();
    }

    public async Task<bool> ExistWithEmailAsync(string email)
    {
        var filter = Builders<User>.Filter.Eq(x => x.Email, email);
        return await _mongo.Users.Find(filter).AnyAsync();
    }

    public async Task UpsertAsync(User user)
    {
        var filter = Builders<User>.Filter.Eq(x => x.Identifier, user.Identifier);
        await _mongo.Users.ReplaceOneAsync(filter, user, new ReplaceOptions { IsUpsert = true });
    }
}