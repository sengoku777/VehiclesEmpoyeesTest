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
                _logger.LogInformation("Get all collection on the DB");
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
                _logger.LogInformation("Get collection expression on the DB");
                return Collection.AsNoTracking()
                    .FirstOrDefault(expression);
            }
            catch (NullReferenceException e)
            {
                _logger.LogError(e, $"Error on get Get {typeof(TEntity).FullName}s.");
                throw;
            }
        }

        public async Task<TEntity> AddAsync(TEntity entity)
        {
            try
            {
                _logger.LogInformation("Added new entity to the DB");
                var result = Collection.Add(entity);
                _logger.LogInformation("Save changes async");
                await _context.SaveChangesAsync();
                return result.Entity;
            }
            catch (NullReferenceException e)
            {
                _logger.LogError(e, $"Error on get Add {typeof(TEntity).FullName}s.");
                throw;
            }
        }

        public virtual async Task AddRangeAsync(IEnumerable<TEntity> entity)
        {
            try
            {
                _logger.LogInformation("Added range entity to the DB");
                Collection.AddRange(entity);
                _logger.LogInformation("Save changes async");
                await _context.SaveChangesAsync();
            }
            catch (Exception e)
            {
                _logger.LogError(e, $"Error on Add new {typeof(TEntity).FullName}: {entity.ToString()}.");
                throw;
            }
        }

        public virtual async Task<TEntity> UpdateAsync(TEntity entity)
        {
            try
            {
                _logger.LogInformation("Updating entity on the DB");
                entity = Collection.Update(entity).Entity;
                _logger.LogInformation("Save changes async");
                await _context.SaveChangesAsync();
                return entity;
            }
            catch (Exception e)
            {
                _logger.LogError(e, $"Error on Update {typeof(TEntity).FullName}: {entity.ToString()}.");
                throw;
            }
        }

        public virtual async Task DeleteAsync(Func<TEntity, bool> predicator)
        {
            try
            {
                _logger.LogInformation("Deliting entity on the DB");
                var entities = Find(predicator);
                Collection.RemoveRange(entities);
                _logger.LogInformation("Save changes async");
                await _context.SaveChangesAsync();
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
                _logger.LogInformation("Finding collection expression on the DB");
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
                _logger.LogInformation("Where collection expression on the DB");
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
