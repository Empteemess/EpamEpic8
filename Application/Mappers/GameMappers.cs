using Application.Dtos;
using Domain.Entities;

namespace Application.Mappers;

public static class GameMappers
{
    public static Game ToGame(this AddGameDto gameDto)
    {
        var game = new Game
        {
            Name = gameDto.Name,
            Price = gameDto.Price,
        };
        
        return game;
    }
}