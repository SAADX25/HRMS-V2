using Domain.Interfaces;
using Infrastructure.Data;
using Infrastructure.Data.Repositories;

namespace Infrastructure.Repositories;

public class UnitOfWork(AppDbContext context) : IUnitOfWork
{
    private readonly Dictionary<Type, object> _repositories = [];

    public IGenericRepository<T> Repository<T>() where T : class
    {
        var type = typeof(T);

        if (!_repositories.ContainsKey(type))
            _repositories[type] = new GenericRepository<T>(context);

        return (IGenericRepository<T>)_repositories[type];
    }

    public async Task<int> SaveChangesAsync() =>
        await context.SaveChangesAsync();

    public void Dispose() => context.Dispose();
}