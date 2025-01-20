using Domain.Entities;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Infrastructure.Repositories;

public class GameRepository : IGameRepository
{
    private readonly MongoDbContext _mongoDbContext;
    private readonly AppDbContext _context;

    public GameRepository(MongoDbContext mongoDbContext,AppDbContext context)
    {
        _mongoDbContext = mongoDbContext;
        _context = context;
    }

    public async Task<Game> GetGameByIdAsync(ObjectId id)
    {
        var game = await _mongoDbContext.Game
            .FindAsync(x => x.Id == id);
        return await game.FirstOrDefaultAsync();
    }
    public async Task DeleteGameByIdAsync(ObjectId id)
    {
        using var transaction = await _context.Database.BeginTransactionAsync();
        try
        {
            var game = await _mongoDbContext.Game.FindAsync(x => x.Id == id);
            _context.Games.Remove(game.FirstOrDefault());
            await _context.SaveChangesAsync();
        
            var filter = Builders<Game>.Filter.Eq(x => x.Id, id);
            await _mongoDbContext.Game.DeleteOneAsync(filter);
        
            await transaction.CommitAsync();
        }
        catch (Exception e)
        {
            await _context.Database.RollbackTransactionAsync();
            throw new InvalidDataException(e.Message);//RandomExc
        }
    }
    public async Task<IEnumerable<Game>> GetAllGamesAsync()
    {
        var result = await _mongoDbContext.Game.Find( _ => true ).ToListAsync();
        return result;
    }
    
    public async Task AddGamesAsync(Game game)
    {
        using var transaction = await _context.Database.BeginTransactionAsync();
        try
        {
            await _context.Games.AddAsync(game);
            await _context.SaveChangesAsync();
        
            await _mongoDbContext.Game.InsertOneAsync(game);
        
            await transaction.CommitAsync();
        }
        catch (Exception e)
        {
            await _context.Database.RollbackTransactionAsync();
            throw new InvalidDataException(e.Message);//RandomExc
        }
    }
}