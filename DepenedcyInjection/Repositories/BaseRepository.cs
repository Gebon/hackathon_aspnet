using System;
using DependencyInjection.Models;

namespace DepenedcyInjection.Repositories
{
    public abstract class BaseRepository : IDisposable
    {
        protected readonly ApplicationDbContext context;
        private bool disposed;
        public BaseRepository(ApplicationDbContext context)
        {
            this.context = context;
        }
        


        protected virtual void Dispose(bool disposing)
        {
            if (disposed) return;
            if (disposing)
            {
                context.Dispose();
            }
            disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}