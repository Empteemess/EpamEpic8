using Domain.Entities;
using Domain.IRepositories;
using Infrastructure.DbConfigs;
using Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;

namespace Infrastructure.Data;

public class AppDbContext : DbContext , IMysqlContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
        
    }

    public DbSet<Category> Categories { get; set; }
    public DbSet<GameCategory> GameCategories { get; set; }
    public DbSet<Game> Games { get; set; }
    public IDbContextTransaction Transaction { get; set; }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfiguration(new GameCategoryConfigurations());
        modelBuilder.ApplyConfiguration(new GameConfiguration());
        modelBuilder.ApplyConfiguration(new CategoryConfiguration());
    }
}