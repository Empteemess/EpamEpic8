using Application.Dtos.Categories;
using Domain.Entities;

namespace Application.Dtos;

public class GameDto
{
    public string Id { get; set; }
    public string Name { get; set; }
    public double Price { get; set; }
    
    public IEnumerable<GetCategoryDto> Categories { get; set; }
}