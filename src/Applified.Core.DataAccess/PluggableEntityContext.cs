using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Applified.Core.DataAccess.Contracts;

namespace Applified.Core.DataAccess
{
    public class PluggableEntityContext : EntityContext
    {
        private readonly IModelBuilder[] _modelBuilders;

        public PluggableEntityContext(
            IModelBuilder[] modelBuilders
            )
        {
            _modelBuilders = modelBuilders;
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            if (_modelBuilders != null)
            {
                foreach (var builder in _modelBuilders)
                {
                    builder.OnModelCreating(modelBuilder);
                }
            }
        }
    }
}
