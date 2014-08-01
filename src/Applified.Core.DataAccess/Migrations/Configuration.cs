using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using Applified.Core.Entities.Infrastructure;

namespace Applified.Core.DataAccess.Migrations
{
    internal sealed class Configuration : DbMigrationsConfiguration<EntityContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
            AutomaticMigrationDataLossAllowed = true;
        }

        protected override void Seed(EntityContext context)
        {

        }
    }
}
