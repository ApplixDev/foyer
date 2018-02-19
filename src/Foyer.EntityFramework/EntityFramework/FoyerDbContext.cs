using System.Data.Common;
using Abp.Zero.EntityFramework;
using Foyer.Authorization.Roles;
using Foyer.Authorization.Users;
using Foyer.MultiTenancy;

namespace Foyer.EntityFramework
{
    public class FoyerDbContext : AbpZeroDbContext<Tenant, Role, User>
    {
        //TODO: Define an IDbSet for your Entities...

        /* NOTE: 
         *   Setting "Default" to base class helps us when working migration commands on Package Manager Console.
         *   But it may cause problems when working Migrate.exe of EF. If you will apply migrations on command line, do not
         *   pass connection string name to base classes. ABP works either way.
         */
        public FoyerDbContext()
            : base("Default")
        {

        }

        /* NOTE:
         *   This constructor is used by ABP to pass connection string defined in FoyerDataModule.PreInitialize.
         *   Notice that, actually you will not directly create an instance of FoyerDbContext since ABP automatically handles it.
         */
        public FoyerDbContext(string nameOrConnectionString)
            : base(nameOrConnectionString)
        {

        }

        //This constructor is used in tests
        public FoyerDbContext(DbConnection existingConnection)
         : base(existingConnection, false)
        {

        }

        public FoyerDbContext(DbConnection existingConnection, bool contextOwnsConnection)
         : base(existingConnection, contextOwnsConnection)
        {

        }
    }
}
