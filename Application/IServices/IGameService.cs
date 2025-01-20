using Application.Dtos;
using Domain.Entities;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Application.Services;

public interface IGameService
{
    Task DeleteGameAsync(string gameId);
    Task<GameDto> GetGameByIdAsync(string gameId);
    Task AddGameAsync(AddGameDto addGameDto);
    Task<IEnumerable<GameDto>> GetAllGamesAsync();    
}