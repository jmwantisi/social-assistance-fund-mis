using System.Linq.Expressions;
using socialAssistanceFundMIS.Common;

namespace socialAssistanceFundMIS.Infrastructure.Repositories
{
    public interface IRepository<T> where T : class
    {
        Task<Result<T>> GetByIdAsync(int id);
        Task<Result<IEnumerable<T>>> GetAllAsync();
        Task<Result<IEnumerable<T>>> FindAsync(Expression<Func<T, bool>> predicate);
        Task<Result<T>> FirstOrDefaultAsync(Expression<Func<T, bool>> predicate);
        Task<Result<bool>> ExistsAsync(Expression<Func<T, bool>> predicate);
        Task<Result<T>> AddAsync(T entity);
        Task<Result<IEnumerable<T>>> AddRangeAsync(IEnumerable<T> entities);
        Task<Result<T>> UpdateAsync(T entity);
        Task<Result> DeleteAsync(T entity);
        Task<Result> DeleteRangeAsync(IEnumerable<T> entities);
        Task<Result<int>> CountAsync(Expression<Func<T, bool>> predicate);
        Task<Result<IEnumerable<T>>> GetPagedAsync(int page, int pageSize, Expression<Func<T, bool>>? predicate = null, Expression<Func<T, object>>? orderBy = null, bool ascending = true);
    }
}

