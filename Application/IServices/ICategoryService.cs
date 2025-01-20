namespace Application.Services;

public interface ICategoryService
{
    Task RemoveCategoryFromGameAsync(string gameId, string categoryId);
    Task AddCategory(AddCategoryToGameDto addCategoryDto);
}