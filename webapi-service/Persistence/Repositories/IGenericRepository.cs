using webapi.Models;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace webapi.Persistence.Repositories
{
    public interface IGenericRepository<T> where T : class
    {
        public Task<IEnumerable<T>> GetAllAsync();

        public Task<T> GetByIdAsync(int id);

        public T GetById(int id);

        public Task<T> CreateAsync(T entity);

        public T Create(T entity);

        public Task UpdateAsync(T entity);

        public void Update(T entity);

        public Task DeleteAsync(T entity);

        public Task DeleteByIdAsync(object id);

        public IDbContextTransaction BeginTransaction();

        public void CommitTransaction();

        public void RollbackTransaction();

        public Task<bool> UpdateDbEntryAsync<E>(E entity, params Expression<Func<E, object>>[] properties) where E : class;


    }
}
