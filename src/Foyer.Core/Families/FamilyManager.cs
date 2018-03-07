using Abp.Domain.Services;
using Foyer.People;

namespace Foyer.Families
{
    public class FamilyManager : DomainService, IFamilyManager
    {
        public void AssignPersonHeadOfFamily(Person person, Family family)
        {
            if (family.HeadOfFamilyId == person.Id)
            {
                return;
            }

            family.HeadOfFamilyId = person.Id;
        }
    }
}
