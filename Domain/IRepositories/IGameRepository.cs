using Domain.Entities;
using MongoDB.Bson;

namespace Domain.IRepositories;

public interface IGameRepository
{
    Task DeleteGameByIdAsync(ObjectId id);
    Task<IEnumerable<Game>> GetAllGamesAsync();
    Task<Game> GetGameByIdAsync(ObjectId id);
    Task AddGamesAsync(Game game);
}