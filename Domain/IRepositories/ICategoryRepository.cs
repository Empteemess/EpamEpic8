using Domain.Entities;

namespace Infrastructure.Repositories;

public interface ICategoryRepository
{
    Task AddCategoryToGame(string gameId, Category category);
    Task RemoveCategoryFromGame(string gameId, string categoryId);
}