using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Cars_Test.Data.Repository.Base
{
    public interface IDataRepository<TEntity> where TEntity : class
    {
        IEnumerable<TEntity> All();

        TEntity Get(Func<TEntity, bool> expression);

        Task<TEntity> AddAsync(TEntity entity);

        Task AddRangeAsync(IEnumerable<TEntity> entities);

        IEnumerable<TEntity> Find(Func<TEntity, bool> expression);

        IQueryable<TEntity> Where(Expression<Func<TEntity, bool>> expression);

        Task<TEntity> UpdateAsync(TEntity entity);

        Task DeleteAsync(Func<TEntity, bool> expression);

        DbSet<TEntity> Collection { get; }
    }
}
