using Abp.Domain.Services;

namespace Foyer
{
    /// <summary>
    /// Derive your doamin services from this class.
    /// </summary>
    public abstract class FoyerDomainServiceBase : DomainService
    {
        protected FoyerDomainServiceBase()
        {
            LocalizationSourceName = FoyerConsts.LocalizationSourceName;
        }
    }
}
