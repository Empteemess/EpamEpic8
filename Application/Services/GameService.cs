using Application.Dtos;
using Application.Mappers;
using Domain.Entities;
using Domain.IRepositories;
using Infrastructure.Repositories;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Application.Services;

public class GameService : IGameService
{
    private readonly IUnitOfWork _unitOfWork;

    public GameService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<GameDto> GetGameByIdAsync(string gameId)
    {
        var games = await _unitOfWork.GameRepository.GetGameByIdAsync(ObjectId.Parse(gameId));

        return new GameDto
        {
            Id = games.Id.ToString(),
            Name = games.Name,
            Price = games.Price,
            Categories = games.Categories.Select(x => x.ToCategoryDto()),
        };
    }

    public async Task<IEnumerable<GameDto>> GetAllGamesAsync()
    {
        var games = await _unitOfWork.GameRepository.GetAllGamesAsync();

        return games.Select(x => new GameDto
        {
            Id = x.Id.ToString(),
            Name = x.Name,
            Price = x.Price,
            Categories = x.Categories.Select(x => x.ToCategoryDto())
        });
    }
    public async Task DeleteGameAsync(string gameId)
    {
        await _unitOfWork.GameRepository.DeleteGameByIdAsync(ObjectId.Parse(gameId));
    }
    public async Task AddGameAsync(AddGameDto addGameDto)
    {
        await _unitOfWork.GameRepository.AddGamesAsync(addGameDto.ToGame());
    }
}