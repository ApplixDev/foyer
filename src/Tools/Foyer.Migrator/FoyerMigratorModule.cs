using System.Data.Entity;
using System.Reflection;
using Abp.Modules;
using Foyer.EntityFramework;

namespace Foyer.Migrator
{
    [DependsOn(typeof(FoyerDataModule))]
    public class FoyerMigratorModule : AbpModule
    {
        public override void PreInitialize()
        {
            Database.SetInitializer<FoyerDbContext>(null);

            Configuration.BackgroundJobs.IsJobExecutionEnabled = false;
        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(Assembly.GetExecutingAssembly());
        }
    }
}