using Abp.Domain.Services;
using Foyer.People;

namespace Foyer.Families
{
    public class FamilyManager : DomainService, IFamilyManager
    {
        public void AssignFamilyFather(Family family, Person father)
        {
            if (family.HusbandId == father.Id)
            {
                return;
            }

            family.HusbandId = father.Id;
        }

        public void AssignFamilyMother(Family family, Person mother)
        {
            if (family.WifeId == mother.Id)
            {
                return;
            }

            family.WifeId = mother.Id;
        }
    }
}
