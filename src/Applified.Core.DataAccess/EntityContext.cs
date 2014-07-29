using System.Data;
using System.Data.Common;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using Applified.Core.DataAccess.Contracts;
using Applified.Core.DataAccess.Conventions;
using Applified.Core.Entities.Identity;
using Applified.Core.Entities.Infrastructure;

namespace Applified.Core.DataAccess
{
    public class EntityContext : DbContext, IDbContext
    {
        public DbSet<StoredObject> StoredObjects { get; set; }

        public DbSet<Application> Applications { get; set; }

        public DbSet<WellKnownApplication> WellKnownApplications { get; set; } 

        public DbSet<Binding> Bindings { get; set; }

        public DbSet<Deployment> Deployments { get; set; }

        public DbSet<UserAccount> UserAccounts { get; set; }

        public DbSet<UserClaim> Claims { get; set; }

        public DbSet<Role> Roles { get; set; }

        public DbSet<UserLogin> UserLogins { get; set; }

        public DbSet<UserAccountRoleMapping> UserAccountRoleMappings { get; set; }

        public DbSet<OAuthClient> OAuthClients { get; set; }

        public DbSet<ExternalOAuthProvider> ExternalOAuthProviders { get; set; }

        public DbSet<ApplicationTemplate> ApplicationTemplates { get; set; }

        public DbSet<Feature> Features { get; set; }

        public DbSet<GlobalFeatureSetting> GlobalFeatureSettings { get; set; }

        public DbSet<FeatureApplicationMapping> FeatureApplicationMappings { get; set; }

        public DbSet<ApplicationFeatureSetting> ApplicationFeatureSettings { get; set; } 

        public EntityContext()
        {
            Configuration.LazyLoadingEnabled = false;
            Configuration.AutoDetectChangesEnabled = false;
            Configuration.ProxyCreationEnabled = false;
            Configuration.ValidateOnSaveEnabled = false;
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
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
