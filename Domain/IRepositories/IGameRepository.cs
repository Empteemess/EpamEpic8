using Domain.Entities;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Infrastructure.Repositories;

public interface IGameRepository
{
    Task DeleteGameByIdAsync(ObjectId id);
    Task<IEnumerable<Game>> GetAllGamesAsync();
    Task<Game> GetGameByIdAsync(ObjectId id);
    Task AddGamesAsync(Game game);
}