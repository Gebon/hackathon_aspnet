using System.Linq;

namespace DepenedcyInjection.Repositories
{
    public interface IRepository<T>
    {
        IQueryable<T> Items { get; }
        void Add(T item);
        void Update(T item);
        void Remove(T item);
        void SaveChanges();
    }
}