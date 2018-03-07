using Abp.Domain.Services;
using Foyer.People;

namespace Foyer.Families
{
    public interface IFamilyManager : IDomainService
    {
        void AssignPersonHeadOfFamily(Person person, Family family);
    }
}
