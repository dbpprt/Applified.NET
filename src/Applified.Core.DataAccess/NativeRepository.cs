using System.Linq;
using Applified.Core.DataAccess.Contracts;

namespace Applified.Core.DataAccess
{
    public class NativeRepository<TEntity> : Repository<TEntity>, INativeRepository<TEntity> where TEntity : class
    {
        public NativeRepository(IDbContext context)
            : base(context) { }

        public override void BeforeAdd(TEntity entity) { }
        public override void BeforeUpdate(TEntity update) { }
        public override void BeforeDelete(TEntity entity) { }

        public override void AfterAdd(TEntity entity) { }
        public override void AfterUpdate(TEntity update) { }
        public override void AfterDelete(TEntity entity) { }

        public override IQueryable<TEntity> Query()
        {
            return DbSet;
        }
    }
}
