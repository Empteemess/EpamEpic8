using Domain.IRepositories;

namespace Infrastructure.Data;

public class CustomDatabase : ICustomDatabase
{
    public CustomDatabase(IMongoDbContext mongoDbContext,
        IMysqlContext mysqlContext)
    {
        MongoDbContext = mongoDbContext;
        MysqlContext = mysqlContext;
    }
    public IMongoDbContext MongoDbContext { get; }
    public IMysqlContext MysqlContext { get; }
}