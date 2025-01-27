using Domain.Entities;

namespace Domain.IRepositories;

public interface ICategoryRepository
{
    Task AddCategoryToGame(string gameId, Category category);
    Task RemoveCategoryFromGame(string gameId, string categoryId);
}