using Application.Dtos.Categories;
using Domain.Entities;
using MongoDB.Bson;

namespace Application.Mappers;

public static class CategoryMappers
{
    public static GetCategoryDto ToCategoryDto(this Category category)
    {
        return new GetCategoryDto(){Id = category.Id.ToString(), Name = category.Name};
    }
    public static Category ToCategory(this AddCategoryToGameDto addCategoryDto)
    {
        return new Category{Name = addCategoryDto.Name};
    }
}