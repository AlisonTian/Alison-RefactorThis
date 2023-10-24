using Microsoft.EntityFrameworkCore;
using RefactorThis.Dtos;
using RefactorThis.Models;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace RefactorThis.Services
{
    public class BaseService<T> : IDisposable, IBaseService<T> where T : class, new()
    {
        protected ProductDbContext _dbContext;
        protected readonly DbSet<T> _dbSet;
        public BaseService(ProductDbContext db)
        {
            _dbContext = db;
            _dbSet = _dbContext.Set<T>();
        }

        public async Task<PaginatedResultDto<T>> GetEntitiesAsync(Expression<Func<T, bool>> whereLambda, int pageIndex = 1, int pageSize = 10, Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null)
        {
            IQueryable<T> query = _dbSet.Where(whereLambda);

            // Apply ordering if an order expression is provided
            if (orderBy != null)
            {
                query = orderBy(query);
            }
            var items = await query.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToListAsync();
            var totalCount = query.Count();
            return new PaginatedResultDto<T>(items, totalCount, pageIndex, pageSize);
        }

        public async Task<T> GetEntityAsync(Expression<Func<T, bool>> whereLambda)
        {
            return await _dbSet.FirstOrDefaultAsync(whereLambda);
        }

        public async Task<T> AddAsync(T entity)
        {
            var result =  await _dbSet.AddAsync(entity);
            await SaveChangesAsync();
            return result.Entity;
        }

        public async Task<bool> UpdateAsync(T entity)
        {
            _dbSet.Attach(entity);
            _dbContext.Entry<T>(entity).State = EntityState.Modified;
            return await SaveChangesAsync();
        }

        public async Task<bool> DeleteAsync(T entity)
        {
            _dbSet.Remove(entity);
            return await SaveChangesAsync();
        }

        public async Task<bool> SaveChangesAsync()
        {
            return await _dbContext.SaveChangesAsync() > 0;
        }

        public void Dispose()
        {
            if (_dbContext != null)
            {
                _dbContext.Dispose();
                _dbContext = null;
            }
        }
    }
}
