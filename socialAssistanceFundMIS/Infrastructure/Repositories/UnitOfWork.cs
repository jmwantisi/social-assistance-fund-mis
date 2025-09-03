using Microsoft.EntityFrameworkCore.Storage;
using socialAssistanceFundMIS.Common;
using socialAssistanceFundMIS.Data;

namespace socialAssistanceFundMIS.Infrastructure.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _context;
        private readonly Dictionary<Type, object> _repositories;
        private IDbContextTransaction? _transaction;
        private bool _disposed = false;

        public UnitOfWork(ApplicationDbContext context)
        {
            _context = context;
            _repositories = new Dictionary<Type, object>();
        }

        public IRepository<T> Repository<T>() where T : class
        {
            var type = typeof(T);
            if (!_repositories.ContainsKey(type))
            {
                _repositories[type] = new Repository<T>(_context);
            }
            return (IRepository<T>)_repositories[type];
        }

        public async Task<Result<int>> SaveChangesAsync()
        {
            try
            {
                var result = await _context.SaveChangesAsync();
                return Result<int>.Success(result);
            }
            catch (Exception ex)
            {
                return Result<int>.Failure($"Error saving changes: {ex.Message}");
            }
        }

        public async Task<Result> BeginTransactionAsync()
        {
            try
            {
                if (_transaction != null)
                {
                    return Result.Failure("Transaction already in progress");
                }

                _transaction = await _context.Database.BeginTransactionAsync();
                return Result.Success();
            }
            catch (Exception ex)
            {
                return Result.Failure($"Error beginning transaction: {ex.Message}");
            }
        }

        public async Task<Result> CommitTransactionAsync()
        {
            try
            {
                if (_transaction == null)
                {
                    return Result.Failure("No transaction in progress");
                }

                await _transaction.CommitAsync();
                await _transaction.DisposeAsync();
                _transaction = null;
                return Result.Success();
            }
            catch (Exception ex)
            {
                return Result.Failure($"Error committing transaction: {ex.Message}");
            }
        }

        public async Task<Result> RollbackTransactionAsync()
        {
            try
            {
                if (_transaction == null)
                {
                    return Result.Failure("No transaction in progress");
                }

                await _transaction.RollbackAsync();
                await _transaction.DisposeAsync();
                _transaction = null;
                return Result.Success();
            }
            catch (Exception ex)
            {
                return Result.Failure($"Error rolling back transaction: {ex.Message}");
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed && disposing)
            {
                _transaction?.Dispose();
                _context.Dispose();
                _disposed = true;
            }
        }
    }
}

