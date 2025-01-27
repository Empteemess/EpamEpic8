using Domain;
using Domain.Entities;
using Domain.IRepositories;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using Sprache;

namespace Infrastructure.Data;

public class MongoDbContext : IMongoDbContext
{
    private readonly IMongoDatabase _gameDb;
    private readonly IMongoDatabase _categoryDb;

    public MongoDbContext(IOptions<MongoDbSettings> mongoDbSettings, IConfiguration config)
    {
        var configuration = config;
        var conString = config["MONGO_CONNECTION_STRING"];

        var gameDbName = config["GAME_DB_NAME"];
        var categoryDbName = config["CATEGORY_DB_NAME"];
        
        var client = new MongoClient(conString);
        _gameDb = client.GetDatabase(gameDbName);
        _categoryDb = client.GetDatabase(categoryDbName);
    }
    public IMongoCollection<Game> Game => _gameDb.GetCollection<Game>(nameof(Game));
    public IMongoCollection<Category> Category => _categoryDb.GetCollection<Category>(nameof(Category));
}