namespace Domain.IRepositories;

public interface IUnitOfWork
{
    Task CommitTransactionAsync();
    Task RollbackTransactionAsync();
    Task SaveChangesAsync();
    Task BeginTransactionAsync();
    public ICategoryRepository CategoryRepository { get; set; }
    public IGameRepository GameRepository { get; set; }
}