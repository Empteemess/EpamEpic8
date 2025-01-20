namespace Domain.Entities;

public class GameCategory
{
    public Guid Id { get; set; }
    
    public Guid GameId { get; set; }
    public Game? Game { get; set; }
    
    public Guid CategoryId { get; set; }
    public Category? Category { get; set; }
}