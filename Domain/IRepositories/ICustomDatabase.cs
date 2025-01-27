namespace Domain.IRepositories;

public interface ICustomDatabase
{
    IMongoDbContext MongoDbContext { get; }
    IMysqlContext MysqlContext { get; }
}