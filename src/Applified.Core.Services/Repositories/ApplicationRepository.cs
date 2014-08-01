using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Applified.Common;
using Applified.Core.DataAccess;
using Applified.Core.DataAccess.Contracts;
using Applified.Core.Entities.Infrastructure;

namespace Applified.Core.Services.Repositories
{
    public class ApplicationRepository : Repository<Application>
    {
        public ApplicationRepository(IDbContext context) 
            : base(context)
        {

        }

        public override void BeforeAdd(Application entity)
        {
            base.BeforeAdd(entity);

            if (string.IsNullOrEmpty(entity.AccessToken))
            {
                entity.AccessToken = RandomTokenGenerator.Generate();
            }
        }
    }
}
