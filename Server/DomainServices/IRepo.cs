using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Server.DomainServices;

public interface IRepo<T>
{
    public void Add(T item);
    public Task AddRangeAsync(IEnumerable<T> item);
    public void Remove(T item);
    public void Update(T item);
    public T GetWhereId(int id);
    public Task<IEnumerable<T>> GetAllAsync();
    public IQueryable<T> GetQueryable();
    public int Count();
}