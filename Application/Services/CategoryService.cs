using Application.Mappers;
using Domain.Entities;
using Domain.IRepositories;
using Infrastructure.Repositories;

namespace Application.Services;

public class CategoryService : ICategoryService
{
    private readonly IUnitOfWork _unitOfWork;

    public CategoryService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task AddCategory(AddCategoryToGameDto addCategoryDto)
    {
        await _unitOfWork.CategoryRepository.AddCategoryToGame(addCategoryDto.GameId, addCategoryDto.ToCategory());
    }

    public async Task RemoveCategoryFromGameAsync(string gameId, string categoryId)
    {
        await _unitOfWork.CategoryRepository.RemoveCategoryFromGame(gameId, categoryId);
    }
}