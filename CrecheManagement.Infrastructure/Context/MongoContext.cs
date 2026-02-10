using CrecheManagement.Domain.Models;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;

namespace CrecheManagement.Infrastructure.Context;

public class MongoContext
{
    private IMongoClient _client;
    private IMongoDatabase _db;

    public MongoContext(IConfiguration configuration)
    {
        var cnnStr = configuration["MongoDB:ConnectionString"];
        var dbName = configuration["MongoDB:DatabaseName"];

        _client = new MongoClient(cnnStr);
        _db = _client.GetDatabase(dbName);
    }

    public IMongoCollection<Creche> Creches => _db.GetCollection<Creche>("Creches");
    public IMongoCollection<User> Users => _db.GetCollection<User>("Users");
}