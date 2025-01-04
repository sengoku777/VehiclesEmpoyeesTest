using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Cars_Test.Data.Repository.Base
{
    public interface IDataRepository<TEntity> where TEntity : class
    {
        IEnumerable<TEntity> All();

        TEntity Get(Func<TEntity, bool> expression);

        TEntity Add(TEntity entity);

        void AddRange(IEnumerable<TEntity> entities);

        IEnumerable<TEntity> Find(Func<TEntity, bool> expression);

        IQueryable<TEntity> Where(Expression<Func<TEntity, bool>> expression);

        TEntity Update(TEntity entity);

        void Delete(Func<TEntity, bool> expression);

        DbSet<TEntity> Collection { get; }
    }
}
