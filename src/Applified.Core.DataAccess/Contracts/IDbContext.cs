using System.Data;
using System.Data.Common;
using System.Data.Entity;
using System.Threading.Tasks;

namespace Applified.Core.DataAccess.Contracts
{
    public interface IDbContext
    {
        IDbSet<T> Set<T>() where T : class;
        int SaveChanges();
        Task<int> SaveChangesAsync();
        void SetState(object o, EntityState state);
        EntityState GetState(object o);
        void Dispose();
        void UseTransaction(DbTransaction transaction);
        DbContextTransaction BeginTransaction(IsolationLevel isolationLevel);
    }
}
