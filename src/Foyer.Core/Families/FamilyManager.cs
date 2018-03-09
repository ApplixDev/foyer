using Abp.Domain.Services;
using Foyer.People;

namespace Foyer.Families
{
    public class FamilyManager : DomainService, IFamilyManager
    {
        public void AssignPersonHeadOfFamily(Person person, Family family)
        {
            if (family.HusbandId == person.Id)
            {
                return;
            }

            family.HusbandId = person.Id;
        }
    }
}
