using Microsoft.EntityFrameworkCore.ChangeTracking;
using RefactorThis.Dtos;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace RefactorThis.Services
{
    public interface IBaseService<T> where T : class, new()
    {
        Task<PaginatedResultDto<T>> GetEntitiesAsync(Expression<Func<T, bool>> whereLambda, int pageIndex = 1, int pageSize = 10, Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null);
        Task<T> GetEntityAsync(Expression<Func<T, bool>> whereLambda);
        Task<T> AddAsync(T entity);
        Task<bool> UpdateAsync(T entity);
        Task<bool> SaveChangesAsync();
        Task<bool> DeleteAsync(T entity);
    }
}
