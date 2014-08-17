#region Copyright (C) 2014 Applified.NET 
// Copyright (C) 2014 Applified.NET
// http://www.applified.net

// This file is part of Applified.NET.

// Applified.NET is free software: you can redistribute it and/or modify
// it under the terms of the GNU Affero General Public License as
// published by the Free Software Foundation, either version 3 of the
// License, or (at your option) any later version.

// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU Affero General Public License for more details.

// You should have received a copy of the GNU Affero General Public License
// along with this program. If not, see <http://www.gnu.org/licenses/>.
#endregion

using System.Data;
using System.Data.Common;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using Applified.Core.DataAccess.Contracts;
using Applified.Core.DataAccess.Conventions;
using Applified.Core.Entities.Infrastructure;

namespace Applified.Core.DataAccess
{
    public class EntityContext : DbContext, IDbContext
    {
        static EntityContext()
        {
            Database.SetInitializer(new NullDatabaseInitializer<EntityContext>());
        }

        public EntityContext() : base("EntityContext")
        {
            Configuration.LazyLoadingEnabled = false;
            Configuration.AutoDetectChangesEnabled = false;
            Configuration.ProxyCreationEnabled = false;
            Configuration.ValidateOnSaveEnabled = false;
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<StoredObject>().ToTable("core.StoredObjects");
            modelBuilder.Entity<Application>().ToTable("core.Applications");
            modelBuilder.Entity<WellKnownApplication>().ToTable("core.WellKnownApplications");
            modelBuilder.Entity<Binding>().ToTable("core.Bindings");
            modelBuilder.Entity<Deployment>().ToTable("core.Deployments");
            modelBuilder.Entity<ApplicationTemplate>().ToTable("core.ApplicationTemplates");
            modelBuilder.Entity<GlobalFeatureSetting>().ToTable("core.GlobalFeatureSettings");
            modelBuilder.Entity<FeatureApplicationMapping>().ToTable("core.FeatureApplicationMappings");
            modelBuilder.Entity<ApplicationFeatureSetting>().ToTable("core.ApplicationFeatureSettings");
            modelBuilder.Entity<Feature>().ToTable("core.Features");
            modelBuilder.Entity<ActiveFarmFeature>().ToTable("core.ActiveFarmFeatures");

            modelBuilder.Conventions.Add(new DateTime2Convention());
            modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();
        }

        public new IDbSet<T> Set<T>() where T : class
        {
            return base.Set<T>();
        }

        public void SetState(object o, EntityState state)
        {
            Entry(o).State = state;
        }

        public EntityState GetState(object o)
        {
            return Entry(o).State;
        }

        public void UseTransaction(DbTransaction transaction)
        {
            Database.UseTransaction(transaction);
        }

        public DbContextTransaction BeginTransaction(IsolationLevel isolationLevel)
        {
            return Database.BeginTransaction(isolationLevel);
        }
    }
}
