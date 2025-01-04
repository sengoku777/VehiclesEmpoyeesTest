using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Cars_Test.Data.Repository.Base
{
    public abstract class BaseRepository<TEntity, TContext>
        : IDataRepository<TEntity>
        where TEntity : class
        where TContext : DbContext
    {
        public DbSet<TEntity> Collection { get; }

        protected readonly ILogger _logger;
        protected readonly TContext _context;

        public BaseRepository(TContext context, ILogger logger)
        {
            _context = context;
            _logger = logger;
            Collection = context.Set<TEntity>();
        }

        public virtual IEnumerable<TEntity> All()
        {
            try
            {
                return Collection.AsNoTracking().ToList();
            }
            catch (NullReferenceException e)
            {
                _logger.LogError(e, $"Error on get All {typeof(TEntity).FullName}s.");
                throw;
            }
        }

        public TEntity Get(Func<TEntity, bool> expression)
        {
            try
            {
                return Collection.AsNoTracking()
                    .FirstOrDefault(expression);
            }
            catch (NullReferenceException e)
            {
                _logger.LogError(e, $"Error on get Get {typeof(TEntity).FullName}s.");
                throw;
            }
        }

        public TEntity Add(TEntity entity)
        {
            try
            {
                var result = Collection.Add(entity);
                _context.SaveChanges();
                return result.Entity;
            }
            catch (NullReferenceException e)
            {
                _logger.LogError(e, $"Error on get Add {typeof(TEntity).FullName}s.");
                throw;
            }
        }

        public virtual void AddRange(IEnumerable<TEntity> entity)
        {
            try
            {
                Collection.AddRange(entity);
                _context.SaveChanges();
            }
            catch (Exception e)
            {
                _logger.LogError(e, $"Error on Add new {typeof(TEntity).FullName}: {entity.ToString()}.");
                throw;
            }
        }

        public virtual TEntity Update(TEntity entity)
        {
            try
            {
                entity = Collection.Update(entity).Entity;
                _context.SaveChanges();
                return entity;
            }
            catch (Exception e)
            {
                _logger.LogError(e, $"Error on Update {typeof(TEntity).FullName}: {entity.ToString()}.");
                throw;
            }
        }

        public virtual void Delete(Func<TEntity, bool> predicator)
        {
            try
            {
                var entities = Find(predicator);
                Collection.RemoveRange(entities);
                _context.SaveChanges();
            }
            catch (Exception e)
            {
                _logger.LogError(e, $"Error on Delete {typeof(TEntity).FullName}: {predicator.ToString()}.");
                throw;
            }
        }

        public virtual IEnumerable<TEntity> Find(Func<TEntity, bool> predicator)
        {
            try
            {
                return Collection.AsNoTracking().Where(predicator);
            }
            catch (Exception e)
            {
                _logger.LogError(e, $"Error on Find {typeof(TEntity).FullName}: {predicator.ToString()}.");
                throw;
            }
        }

        public IQueryable<TEntity> Where(Expression<Func<TEntity, bool>> predicate)
        {
            try
            {
                return Collection.AsNoTracking().Where(predicate);
            }
            catch (Exception e)
            {
                _logger.LogError(e, $"Error on Where {typeof(TEntity).FullName}: {predicate.ToString()}.");
                throw;
            }
        }
    }
}
