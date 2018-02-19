using Foyer.EntityFramework;
using EntityFramework.DynamicFilters;

namespace Foyer.Migrations.SeedData
{
    public class InitialHostDbBuilder
    {
        private readonly FoyerDbContext _context;

        public InitialHostDbBuilder(FoyerDbContext context)
        {
            _context = context;
        }

        public void Create()
        {
            _context.DisableAllFilters();

            new DefaultEditionsCreator(_context).Create();
            new DefaultLanguagesCreator(_context).Create();
            new HostRoleAndUserCreator(_context).Create();
            new DefaultSettingsCreator(_context).Create();
        }
    }
}
