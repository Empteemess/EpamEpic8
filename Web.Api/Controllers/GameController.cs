using Application.Dtos;
using Application.Services;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Web.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class GameController : ControllerBase
{
    private readonly IGameService _gameService;
    private readonly ICategoryService _categoryService;

    public GameController(IGameService gameService, ICategoryService categoryService)
    {
        _gameService = gameService;
        _categoryService = categoryService;
    }
    
    [HttpDelete("{gameId}")]
    public async Task<IActionResult> DeleteGame(string gameId)
    {
        await _gameService.DeleteGameAsync(gameId);
        return Ok("Game deleted");
    }
    [HttpPost]
    public async Task<IActionResult> AddGame([FromBody]AddGameDto game)
    {
        await _gameService.AddGameAsync(game);
        return Ok("Game added");
    }
    
    [HttpGet("{gameId}")]
    public async Task<IActionResult> GetById(string gameId)
    {
        var game = await _gameService.GetGameByIdAsync(gameId);
        return Ok(game);
    }

    [HttpGet("all")]
    public async Task<IActionResult> GetGames()
    {
        var games = await _gameService.GetAllGamesAsync();
        return Ok(games);
    }
}