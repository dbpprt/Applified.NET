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
            var firstApplication = Guid.NewGuid();
            var secondApplication = Guid.NewGuid();
            var nullGuid = Guid.Empty.ToString();    

            //context.Applications.AddOrUpdate(entity => entity.AccessToken, new Application
            //{
            //    AccessToken = "a",
            //    Id = firstApplication,
            //    Bindings = new List<Binding>
            //    {
            //        new Binding
            //        {
            //            ApplicationId = firstApplication,
            //            Hostname = "dev:8080"
            //        }
            //    }
            //});

            context.Applications.AddOrUpdate(entity => entity.AccessToken, new Application
            {
                AccessToken = "{A5EC5876-3C7E-4A6F-A77E-EDDB416C4437}",
                Id = firstApplication,
                Bindings = new List<Binding>
                {
                    new Binding
                    {
                        ApplicationId = firstApplication,
                        Hostname = "localhost:8080"
                    },
                    new Binding
                    {
                        ApplicationId = firstApplication,
                        Hostname = "127.0.0.1:8080"
                    }
                }
            });

            //context.WellKnownApplications.AddOrUpdate(entity => entity.AccessToken, new WellKnownApplication
            //{
            //    AccessToken = "{871B8E52-63FC-40CD-9392-066AD9219C03}",
            //    Id = secondApplication,
            //    Name = WellKnownNames.Applications.Management,
            //    Description = "Management application avaliable for all applications"
            //});

            context.SaveChanges();
        }
    }
}
