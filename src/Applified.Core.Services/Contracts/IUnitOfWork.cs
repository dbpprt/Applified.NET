using System;
using System.Threading.Tasks;

namespace Applified.Core.Services.Contracts
{
    public interface IUnitOfWork : IDisposable
    {
        void Save();
        Task SaveAsync();
    }
}
