using EM.Domain.Models;
using System.Linq.Expressions;

namespace EM.Repository.Repositories;

public abstract class RepositorioAbstrato<T> where T : IEntidade
{
    public abstract Task AddAsync(T entidade);

    public abstract Task UpdateAsync(T entidade);

    public abstract Task RemoveAsync(T entidade);

    public abstract Task<IEnumerable<T>> GetAllAsync();

    public abstract Task<IEnumerable<T>> GetAsync(Expression<Func<T, bool>> predicate);
}
