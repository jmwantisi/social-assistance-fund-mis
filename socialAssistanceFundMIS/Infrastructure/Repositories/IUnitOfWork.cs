namespace socialAssistanceFundMIS.Infrastructure.Repositories
{
    public interface IUnitOfWork : IDisposable
    {
        IRepository<T> Repository<T>() where T : class;
        Task<Result<int>> SaveChangesAsync();
        Task<Result> BeginTransactionAsync();
        Task<Result> CommitTransactionAsync();
        Task<Result> RollbackTransactionAsync();
    }
}

