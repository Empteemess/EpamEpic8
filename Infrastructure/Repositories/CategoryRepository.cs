using Domain.Entities;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Infrastructure.Repositories;

public class CategoryRepository : ICategoryRepository
{
    private readonly MongoDbContext _mongoDbContext;
    private readonly AppDbContext _context;

    public CategoryRepository(MongoDbContext mongoDbContext,
        AppDbContext context)
    {
        _mongoDbContext = mongoDbContext;
        _context = context;
    }

    public async Task RemoveCategoryFromGame(string gameId, string categoryId)
    {
        using var transaction = _context.Database.BeginTransaction();
        try
        {
            var filter = Builders<Game>.Filter.Eq(x => x.Id, ObjectId.Parse(gameId));
            
            var game = _mongoDbContext.Game.Find(filter).FirstOrDefault();

            var gameFromSql = await _context.Games
                .Include(x => x.GameCategories)
                .FirstOrDefaultAsync(x => x.MySqlId == game.MySqlId);
            
            var update = Builders<Game>.Update.PullFilter(x => x.Categories, c => c.Id == ObjectId.Parse(categoryId));

            await _mongoDbContext.Game.UpdateOneAsync(filter, update);
            
            await transaction.CommitAsync();
        }
        catch (Exception e)
        {
            await transaction.RollbackAsync();
            throw new InvalidDataException(e.Message); //RandomExc
        }
    }

    public async Task AddCategoryToGame(string gameId, Category category)
    {
        using var transaction = await _context.Database.BeginTransactionAsync();
        try
        {
            var filter = Builders<Game>.Filter.Eq(x => x.Id, ObjectId.Parse(gameId));
            var game = await _mongoDbContext.Game.Find(filter).FirstOrDefaultAsync();
        
            if (game == null)
            {
                throw new InvalidDataException("Game not found");
            }

            await _context.Categories.AddAsync(category);
            await _context.Games.AddAsync(game);  

            var gameCategory = new GameCategory()
            {
                GameId = game.MySqlId,  
                CategoryId = category.MySqlId, 
            };
        
            await _context.GameCategories.AddAsync(gameCategory);
            // await _context.SaveChangesAsync();

            category.Id = ObjectId.GenerateNewId();
            var update = Builders<Game>.Update.Push(x => x.Categories, category);
            await _mongoDbContext.Game.UpdateOneAsync(filter, update);

            await transaction.CommitAsync();
        }
        catch (Exception e)
        {
            // Rollback if any errors occur
            await transaction.RollbackAsync();
            throw new InvalidDataException(e.Message);
        }
    }

}