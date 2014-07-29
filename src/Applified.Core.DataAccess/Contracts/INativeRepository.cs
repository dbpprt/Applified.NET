namespace Applified.Core.DataAccess.Contracts
{
    public interface INativeRepository<TEntity> : IRepository<TEntity> where TEntity : class
    {

    }
}
