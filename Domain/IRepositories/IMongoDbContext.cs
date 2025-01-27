using Domain.Entities;
using MongoDB.Driver;

namespace Domain.IRepositories;

public interface IMongoDbContext
{
    IMongoCollection<Game> Game { get; }
    IMongoCollection<Category> Category { get; }
}