using Microsoft.EntityFrameworkCore;
using socialAssistanceFundMIS.Common;
using socialAssistanceFundMIS.Data;
using System.Linq.Expressions;

namespace socialAssistanceFundMIS.Infrastructure.Repositories
{
    public class Repository<T> : IRepository<T> where T : class
    {
        protected readonly ApplicationDbContext _context;
        protected readonly DbSet<T> _dbSet;

        public Repository(ApplicationDbContext context)
        {
            _context = context;
            _dbSet = context.Set<T>();
        }

        public virtual async Task<Result<T>> GetByIdAsync(int id)
        {
            try
            {
                var entity = await _dbSet.FindAsync(id);
                return entity != null ? Result<T>.Success(entity) : Result<T>.Failure("Entity not found");
            }
            catch (Exception ex)
            {
                return Result<T>.Failure($"Error retrieving entity: {ex.Message}");
            }
        }

        public virtual async Task<Result<IEnumerable<T>>> GetAllAsync()
        {
            try
            {
                var entities = await _dbSet.ToListAsync();
                return Result<IEnumerable<T>>.Success(entities);
            }
            catch (Exception ex)
            {
                return Result<IEnumerable<T>>.Failure($"Error retrieving entities: {ex.Message}");
            }
        }

        public virtual async Task<Result<IEnumerable<T>>> FindAsync(Expression<Func<T, bool>> predicate)
        {
            try
            {
                var entities = await _dbSet.Where(predicate).ToListAsync();
                return Result<IEnumerable<T>>.Success(entities);
            }
            catch (Exception ex)
            {
                return Result<IEnumerable<T>>.Failure($"Error finding entities: {ex.Message}");
            }
        }

        public virtual async Task<Result<T>> FirstOrDefaultAsync(Expression<Func<T, bool>> predicate)
        {
            try
            {
                var entity = await _dbSet.FirstOrDefaultAsync(predicate);
                return entity != null ? Result<T>.Success(entity) : Result<T>.Failure("Entity not found");
            }
            catch (Exception ex)
            {
                return Result<T>.Failure($"Error finding entity: {ex.Message}");
            }
        }

        public virtual async Task<Result<bool>> ExistsAsync(Expression<Func<T, bool>> predicate)
        {
            try
            {
                var exists = await _dbSet.AnyAsync(predicate);
                return Result<bool>.Success(exists);
            }
            catch (Exception ex)
            {
                return Result<bool>.Failure($"Error checking existence: {ex.Message}");
            }
        }

        public virtual async Task<Result<T>> AddAsync(T entity)
        {
            try
            {
                var entry = await _dbSet.AddAsync(entity);
                await _context.SaveChangesAsync();
                return Result<T>.Success(entry.Entity);
            }
            catch (Exception ex)
            {
                return Result<T>.Failure($"Error adding entity: {ex.Message}");
            }
        }

        public virtual async Task<Result<IEnumerable<T>>> AddRangeAsync(IEnumerable<T> entities)
        {
            try
            {
                await _dbSet.AddRangeAsync(entities);
                await _context.SaveChangesAsync();
                return Result<IEnumerable<T>>.Success(entities);
            }
            catch (Exception ex)
            {
                return Result<IEnumerable<T>>.Failure($"Error adding entities: {ex.Message}");
            }
        }

        public virtual async Task<Result<T>> UpdateAsync(T entity)
        {
            try
            {
                _dbSet.Update(entity);
                await _context.SaveChangesAsync();
                return Result<T>.Success(entity);
            }
            catch (Exception ex)
            {
                return Result<T>.Failure($"Error updating entity: {ex.Message}");
            }
        }

        public virtual async Task<Result> DeleteAsync(T entity)
        {
            try
            {
                _dbSet.Remove(entity);
                await _context.SaveChangesAsync();
                return Result.Success();
            }
            catch (Exception ex)
            {
                return Result.Failure($"Error deleting entity: {ex.Message}");
            }
        }

        public virtual async Task<Result> DeleteRangeAsync(IEnumerable<T> entities)
        {
            try
            {
                _dbSet.RemoveRange(entities);
                await _context.SaveChangesAsync();
                return Result.Success();
            }
            catch (Exception ex)
            {
                return Result.Failure($"Error deleting entities: {ex.Message}");
            }
        }

        public virtual async Task<Result<int>> CountAsync(Expression<Func<T, bool>> predicate)
        {
            try
            {
                var count = await _dbSet.CountAsync(predicate);
                return Result<int>.Success(count);
            }
            catch (Exception ex)
            {
                return Result<int>.Failure($"Error counting entities: {ex.Message}");
            }
        }

        public virtual async Task<Result<IEnumerable<T>>> GetPagedAsync(int page, int pageSize, Expression<Func<T, bool>>? predicate = null, Expression<Func<T, object>>? orderBy = null, bool ascending = true)
        {
            try
            {
                var query = _dbSet.AsQueryable();

                if (predicate != null)
                    query = query.Where(predicate);

                if (orderBy != null)
                {
                    query = ascending ? query.OrderBy(orderBy) : query.OrderByDescending(orderBy);
                }

                var entities = await query
                    .Skip((page - 1) * pageSize)
                    .Take(pageSize)
                    .ToListAsync();

                return Result<IEnumerable<T>>.Success(entities);
            }
            catch (Exception ex)
            {
                return Result<IEnumerable<T>>.Failure($"Error retrieving paged entities: {ex.Message}");
            }
        }

        protected virtual IQueryable<T> GetQueryable()
        {
            return _dbSet.AsQueryable();
        }
    }
}

