using EM.Domain.Models;
using System.Linq.Expressions;

namespace EM.Repository.Repositories;

public abstract class RepositorioAbstrato<T> where T : IEntidade
{
    public abstract void Add(T entidade);

    public abstract void Update(T entidade);

    public abstract void Remove(T entidade);

    public abstract IEnumerable<T> GetAll();

    public abstract IEnumerable<T> Get(Expression<Func<T, bool>> predicate);
}
