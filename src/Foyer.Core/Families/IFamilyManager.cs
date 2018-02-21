using Abp.Domain.Services;
using Foyer.People;

namespace Foyer.Families
{
    public interface IFamilyManager : IDomainService
    {
        void AssignPersonHeadOfFamily(Family family, Person person);
    }
}
