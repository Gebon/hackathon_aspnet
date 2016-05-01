using System;
using System.Linq;
using DependencyInjection.Models;

namespace DepenedcyInjection.Repositories
{
    public abstract class BaseRepository<T> : IDisposable, IRepository<T>
    {
        protected readonly ApplicationDbContext context;
        private bool disposed;
        public BaseRepository(ApplicationDbContext context)
        {
            this.context = context;
        }

        public abstract void Update(T item);
        public abstract void Remove(T item);

        public void SaveChanges()
        {
            context.SaveChanges();
        }


        protected virtual void Dispose(bool disposing)
        {
            if (disposed) return;
            if (disposing)
            {
                context.Dispose();
                disposed = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public abstract IQueryable<T> Items { get; }
        public abstract void Add(T item);
    }
}