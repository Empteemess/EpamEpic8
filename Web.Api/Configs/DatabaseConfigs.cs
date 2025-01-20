using Application.Services;
using Domain;
using Infrastructure.Data;
using Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Web.Api.Configs;

public static class DatabaseConfigs
{
    public static IServiceCollection AddMySqlConfigs(this IServiceCollection services,IConfiguration configuration)
    {
        services
            .AddDbContext<AppDbContext>(opt =>
            {
                opt.UseSqlServer(configuration["MYSQL_CONNECTION_STRING"]);
            });
        
        return services;
    }
    
    public static IServiceCollection AddMongoDbConfigs(this IServiceCollection services,IConfiguration configuration)
    {
        var mongoDbSettings = new MongoDbSettings
        {
            ConString = configuration["MONGO_CONNECTION_STRING"],
            DatabaseName = configuration["MONGO_DB_NAME"],
        };

        services.AddSingleton(mongoDbSettings);
        services.AddSingleton<MongoDbContext>();
        
        return services;
    }
    
    public static IServiceCollection AddConfigs(this IServiceCollection services)
    {
        services.AddScoped<IGameRepository,GameRepository>();
        services.AddScoped<ICategoryRepository,CategoryRepository>();
        
        services.AddScoped<IUnitOfWork,UnitOfWork>();
        
        services.AddScoped<IGameService,GameService>();
        services.AddScoped<ICategoryService,CategoryService>();
        
        return services;
    }
}