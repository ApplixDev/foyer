using Abp.Domain.Services;
using Foyer.People;

namespace Foyer.Families
{
    public interface IFamilyManager : IDomainService
    {
        void AssignFamilyFather(Family family, Person father);
        void AssignFamilyMother(Family family, Person mother);
    }
}
