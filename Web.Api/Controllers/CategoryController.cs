using Application.Services;
using Domain.IRepositories;
using Infrastructure.Data;
using Infrastructure.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace Web.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class CategoryController : ControllerBase
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICategoryService _categoryService;

    public CategoryController(IUnitOfWork unitOfWork,ICategoryService categoryService)
    {
        _unitOfWork = unitOfWork;
        _categoryService = categoryService;
    }
    [HttpPost]
    public async Task<IActionResult> AddCategory([FromBody] AddCategoryToGameDto addCategoryDto)
    {
        await _categoryService.AddCategory(addCategoryDto);
        return Ok("Category added");
    }
    [HttpDelete("{gameId}/{categoryId}")]
    public async Task<IActionResult> DeleteCategoryFromGame(string gameId, string categoryId)
    {
        await _unitOfWork.CategoryRepository.RemoveCategoryFromGame(gameId, categoryId);
        return Ok("Category deleted");
    }
}