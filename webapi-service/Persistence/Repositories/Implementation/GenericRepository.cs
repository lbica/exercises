using webapi.Exceptions;
using webapi.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Storage;
using System.Linq.Expressions;
using System.Threading.Tasks;


namespace webapi.Persistence.Repositories.Implementation
{
    public class GenericRepository<T> : IGenericRepository<T> where T : BaseEntity, new()
    {
        protected readonly AppDbContext _dbContext;

        public GenericRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<T> CreateAsync(T entity)
        {

            ArgumentNullException.ThrowIfNull(entity);
            //Task.Delay(3000).Wait();
            //Console.WriteLine(_dbContext.ChangeTracker.DebugView.LongView);

            try
            {
                await _dbContext.AddAsync(entity);
                await _dbContext.SaveChangesAsync();


                return entity;
            }

            catch (Exception ex)
            {
                throw new WebApiServiceException($"{nameof(entity)} could not be saved: {ex.InnerException.Message}", ex);
            }

        }

        public async Task DeleteAsync(T entity)
        {
            _dbContext.Remove(entity);
            await _dbContext.SaveChangesAsync();
        }

        public async Task DeleteByIdAsync(object id)
        {
            var entity = new T { Id = (int)id};
            //_dbContext.Attach(entity);
            _dbContext.Remove(entity);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            var result = await _dbContext.Set<T>()
                //.Where(p => p.Id != -1).
                .ToListAsync();
            return result;
        }

        public async Task<T> GetByIdAsync(int id)
        {
            return await _dbContext.Set<T>().FindAsync(id);
        }

        public T GetById(int id)
        {
            return _dbContext.Set<T>().Find(id);
        }

        public async Task UpdateAsync(T entity)
        {

            _dbContext.Entry(entity).State = EntityState.Modified;
            //_dbContext.Update(entity);
            await _dbContext.SaveChangesAsync();
        }


        public IDbContextTransaction BeginTransaction()
        {
            var transaction = _dbContext.Database.BeginTransaction();
            return transaction;
            
        }

        public void CommitTransaction()
        {
            _dbContext.Database.CommitTransaction();

        }

        public void RollbackTransaction()
        {
            _dbContext.Database.RollbackTransaction();

        }

        public void Update(T entity)
        {
            _dbContext.Entry(entity).State = EntityState.Modified;
            //_dbContext.Update(entity);
            _dbContext.SaveChanges();
        }

        public T Create(T entity)
        {
            ArgumentNullException.ThrowIfNull(entity);
            //Task.Delay(3000).Wait();
            //Console.WriteLine(_dbContext.ChangeTracker.DebugView.LongView);

            try
            {
                _dbContext.Add(entity);
                _dbContext.SaveChanges();


                return entity;
            }

            catch (Exception ex)
            {
                throw new WebApiServiceException($"{nameof(entity)} could not be saved: {ex.InnerException.Message}", ex);
            }
        }

        public async Task<bool> UpdateDbEntryAsync<Entity>(Entity entity, params Expression<Func<Entity, object>>[] properties) where Entity : class
        {
            try
            {


                //var entry = _dbContext.Entry(entity);
                var entry = _dbContext.Set<Entity>().Attach(entity);

                // Ensure the entity is being tracked
                //if (entry.State == EntityState.Detached)
                //{
                //    _dbContext.Attach(entity);
                //    entry = _dbContext.Entry(entity); // Refresh entry after attaching
                //}

                foreach (var prop in properties)
                {
                    entry.Property(prop).IsModified = true;

                }

                    

                //entry.State = EntityState.Modified;
                await _dbContext.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("UpdateDbEntryAsync exception: " + ex.Message);
                return false;
            }
        }
    }
}
