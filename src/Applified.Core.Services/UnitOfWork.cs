using System;
using System.Threading.Tasks;
using Applified.Core.DataAccess.Contracts;
using Applified.Core.Services.Contracts;

namespace Applified.Core.Services
{
    internal class UnitOfWork : IUnitOfWork
    {
        private readonly IDbContext _context;

        private bool _disposed;

        public UnitOfWork(IDbContext context)
        {
            _context = context;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public void Save()
        {
            _context.SaveChanges();
        }

        public Task SaveAsync()
        {
            return _context.SaveChangesAsync();
        }

        public void Dispose(bool disposing)
        {
            if (!_disposed)
                if (disposing)
                    _context.Dispose();

            _disposed = true;
        }
    }
}