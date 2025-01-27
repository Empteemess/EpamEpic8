using Domain.Entities;
using Domain.IRepositories;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Infrastructure.Repositories;

public class CategoryRepository : ICategoryRepository
{
    private readonly ICustomDatabase _context;
    private readonly IServiceProvider _serviceProvider;

    public CategoryRepository(ICustomDatabase context, IServiceProvider serviceProvider)
    {
        _context = context;
        _serviceProvider = serviceProvider;
    }

    public async Task RemoveCategoryFromGame(string gameId, string categoryId)
    {

        var scope = _serviceProvider.CreateScope();
        var unitOfWork = scope.ServiceProvider.GetService<IUnitOfWork>();
        
        using var transaction = unitOfWork.BeginTransactionAsync();
        try
        {
            var filter = Builders<Game>.Filter.Eq(x => x.Id, ObjectId.Parse(gameId));

            var game = _context.MongoDbContext.Game.Find(filter).FirstOrDefault();

            var gameFromSql = await _context.MysqlContext.Games
                .Include(x => x.GameCategories)
                .FirstOrDefaultAsync(x => x.MySqlId == game.MySqlId);

            var update = Builders<Game>.Update.PullFilter(x => x.Categories, c => c.Id == ObjectId.Parse(categoryId));

            await _context.MongoDbContext.Game.UpdateOneAsync(filter, update);

            await unitOfWork.CommitTransactionAsync();
        }
        catch (Exception e)
        {
            await unitOfWork.RollbackTransactionAsync();
            throw new InvalidDataException(e.Message); //RandomExc
        }
    }

    public async Task AddCategoryToGame(string gameId, Category category)
    {
        var scope = _serviceProvider.CreateScope();
        var unitOfWork = scope.ServiceProvider.GetService<IUnitOfWork>();
        
        using var transaction = unitOfWork.BeginTransactionAsync();
        try
        {
            var filter = Builders<Game>.Filter.Eq(x => x.Id, ObjectId.Parse(gameId));
            var game = await _context.MongoDbContext.Game.Find(filter).FirstOrDefaultAsync();

            if (game == null)
            {
                throw new InvalidDataException("Game not found");
            }

            await _context.MysqlContext.Categories.AddAsync(category);
            await _context.MysqlContext.Games.AddAsync(game);

            var gameCategory = new GameCategory()
            {
                GameId = game.MySqlId,
                CategoryId = category.MySqlId,
            };

            await _context.MysqlContext.GameCategories.AddAsync(gameCategory);
            // await _context.SaveChangesAsync();

            category.Id = ObjectId.GenerateNewId();
            var update = Builders<Game>.Update.Push(x => x.Categories, category);
            await _context.MongoDbContext.Game.UpdateOneAsync(filter, update);

            await unitOfWork.CommitTransactionAsync();
        }
        catch (Exception e)
        {
            // Rollback if any errors occur
            await unitOfWork.RollbackTransactionAsync();
            throw new InvalidDataException(e.Message);
        }
    }
}