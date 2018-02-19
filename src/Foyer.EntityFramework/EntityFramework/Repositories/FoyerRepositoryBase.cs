using Abp.Domain.Entities;
using Abp.EntityFramework;
using Abp.EntityFramework.Repositories;

namespace Foyer.EntityFramework.Repositories
{
    public abstract class FoyerRepositoryBase<TEntity, TPrimaryKey> : EfRepositoryBase<FoyerDbContext, TEntity, TPrimaryKey>
        where TEntity : class, IEntity<TPrimaryKey>
    {
        protected FoyerRepositoryBase(IDbContextProvider<FoyerDbContext> dbContextProvider)
            : base(dbContextProvider)
        {

        }

        //add common methods for all repositories
    }

    public abstract class FoyerRepositoryBase<TEntity> : FoyerRepositoryBase<TEntity, int>
        where TEntity : class, IEntity<int>
    {
        protected FoyerRepositoryBase(IDbContextProvider<FoyerDbContext> dbContextProvider)
            : base(dbContextProvider)
        {

        }

        //do not add any method here, add to the class above (since this inherits it)
    }
}
