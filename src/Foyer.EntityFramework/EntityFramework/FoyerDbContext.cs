using System.Data.Common;
using System.Data.Entity;
using Abp.Zero.EntityFramework;
using Foyer.Authorization.Roles;
using Foyer.Authorization.Users;
using Foyer.Families;
using Foyer.People;
using Foyer.FamilyRelationships;
using Foyer.MultiTenancy;
using System.IO;
using System;

namespace Foyer.EntityFramework
{
    public class FoyerDbContext : AbpZeroDbContext<Tenant, Role, User>
    {
        #region ctors
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
            //#if DEBUG
            //            Database.Log = message => FileLogger.Log(message);
            //#endif
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
        #endregion

        public virtual IDbSet<Person> People { get; set; }
        public virtual IDbSet<Family> Families { get; set; }
        public virtual IDbSet<FamilyRelationship> FamilyRelationships { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Configurations.Add(new FamilyRelationshipConfiguration());

            base.OnModelCreating(modelBuilder);
        }

        public class FileLogger
        {
            public static void Log(string message)
            {
                string path = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + @"\SqlLog.txt";
                File.AppendAllText(path, message);
            }
        }
    }
}
