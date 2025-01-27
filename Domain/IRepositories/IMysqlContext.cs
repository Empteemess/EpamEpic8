using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace Domain.IRepositories;

public interface IMysqlContext
{
    DatabaseFacade Database { get; }
    public DbSet<Category> Categories { get; set; }
    public DbSet<GameCategory> GameCategories { get; set; }
    public DbSet<Game> Games { get; set; }
}