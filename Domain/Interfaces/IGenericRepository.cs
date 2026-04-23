
// Author: Abedalqader Alfaqeeh 
// last edit : 2026-04-10
// /<summary> this interface is used to define the basic CRUD operations for a generic repository pattern.
// It provides methods for retrieving, updating, and deleting entities of type T, where T is a class.
// The methods include options for including related entities when retrieving data, as well as checking for the existence of an entity by its ID.
// The SaveChangesAsync method is used to persist changes to the database asynchronously.</summary>


using System.Linq.Expressions;

namespace Domain.Interfaces
{
    public interface IGenericRepository<T> where T : class
    {
        Task<IEnumerable<T>> GetAllAsync(
            params Expression<Func<T, object>>[] includes);

        Task<T?> GetAsync(
            Expression<Func<T, bool>> predicate,
            params Expression<Func<T, object>>[] includes);

        IQueryable<T> GetAllQueryable();

        Task<bool> ExistsAsync(Expression<Func<T, bool>> predicate);

        Task AddAsync(T entity);
        void Update(T entity);
        void Delete(T entity);
    }
}
