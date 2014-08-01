using System.Linq;
using Applified.Core.DataAccess;
using Applified.Core.DataAccess.Contracts;
using Applified.Core.Entities.Contracts;
using Applified.Core.ServiceContracts;

namespace Applified.Core.Services.Repositories
{
    public class ApplicationDependantRepository<TEntity> : Repository<TEntity> where TEntity : class, IApplicationDependant
    {
        private readonly ICurrentContext _currentContext;

        public ApplicationDependantRepository(
            IDbContext context,
            ICurrentContext currentContext
            ) 
            : base(context)
        {
            _currentContext = currentContext;
        }

        public override void BeforeAdd(TEntity entity)
        {
            base.BeforeAdd(entity);

            entity.ApplicationId = _currentContext.ApplicationId;
        }

        public override void BeforeUpdate(TEntity update)
        {
            base.BeforeUpdate(update);

            update.ApplicationId = _currentContext.ApplicationId;
        }

        public override void BeforeDelete(TEntity entity)
        {
            entity.ApplicationId = _currentContext.ApplicationId;

            base.BeforeDelete(entity);
        }

        public override IQueryable<TEntity> Query()
        {
            return base.Query()
                .Where(entity => entity.ApplicationId == _currentContext.ApplicationId);
        }
    }
}
