using Domain.Entities;
using Domain.IRepositories;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Infrastructure.Repositories;

public class GameRepository : IGameRepository
{
    private readonly ICustomDatabase _context;
    private readonly IServiceProvider _serviceProvider;

    public GameRepository(ICustomDatabase context,IServiceProvider serviceProvider)
    {
        _context = context;
        _serviceProvider = serviceProvider;
    }

    public async Task<Game> GetGameByIdAsync(ObjectId id)
    {
        var game = await _context.MongoDbContext.Game
            .FindAsync(x => x.Id == id);
        return await game.FirstOrDefaultAsync();
    }

    public async Task DeleteGameByIdAsync(ObjectId id)
    {
        var scope = _serviceProvider.CreateScope();
        var unitOfWork = scope.ServiceProvider.GetService<IUnitOfWork>();
        
        using var transaction = unitOfWork.BeginTransactionAsync();
        try
        {
            var game = await _context.MongoDbContext.Game.FindAsync(x => x.Id == id);
            _context.MysqlContext.Games.Remove(game.FirstOrDefault());
            await unitOfWork.SaveChangesAsync();

            var filter = Builders<Game>.Filter.Eq(x => x.Id, id);
            await _context.MongoDbContext.Game.DeleteOneAsync(filter);

            await unitOfWork.CommitTransactionAsync();
        }
        catch (Exception e)
        {
            await unitOfWork.RollbackTransactionAsync();
            throw new InvalidDataException(e.Message); //RandomExc
        }
    }

    public async Task<IEnumerable<Game>> GetAllGamesAsync()
    {
        var result = await _context.MongoDbContext.Game.Find(_ => true).ToListAsync();
        return result;
    }

    public async Task AddGamesAsync(Game game)
    {
        var scope = _serviceProvider.CreateScope();
        var unitOfWork = scope.ServiceProvider.GetService<IUnitOfWork>();
        
        using var transaction = unitOfWork.BeginTransactionAsync();
        try
        {
            await _context.MysqlContext.Games.AddAsync(game);
            await unitOfWork.SaveChangesAsync();

            await _context.MongoDbContext.Game.InsertOneAsync(game);

            await unitOfWork.CommitTransactionAsync();
        }
        catch (Exception e)
        {
            await unitOfWork.RollbackTransactionAsync();
            throw new InvalidDataException(e.Message); //RandomExc
        }
    }
}