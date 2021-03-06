using System.Data.Entity.Migrations;
using Abp.MultiTenancy;
using Abp.Zero.EntityFramework;
using Foyer.Migrations.SeedData;
using EntityFramework.DynamicFilters;

namespace Foyer.Migrations
{
    public sealed class Configuration : DbMigrationsConfiguration<Foyer.EntityFramework.FoyerDbContext>, IMultiTenantSeed
    {
        public AbpTenantBase Tenant { get; set; }

        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
            ContextKey = "Foyer";
        }

        protected override void Seed(Foyer.EntityFramework.FoyerDbContext context)
        {
            context.DisableAllFilters();

            if (Tenant == null)
            {
                //Host seed
                new InitialHostDbBuilder(context).Create();

                //Default tenant seed (in host database).
                new DefaultTenantCreator(context).Create();
                new TenantRoleAndUserBuilder(context, 1).Create();

                //Used for manual tests
                new FamiliesAndPeopleCreator(context).Create();
            }
            else
            {
                //You can add seed for tenant databases and use Tenant property...
            }

            context.SaveChanges();
        }
    }
}
