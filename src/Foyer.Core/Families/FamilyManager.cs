using Abp.Domain.Services;
using Foyer.People;
using System;

namespace Foyer.Families
{
    public class FamilyManager : DomainService, IFamilyManager
    {
        public FamilyManager()
        {

        }

        public void AssignPersonHeadOfFamily(Family family, Person person)
        {
            if (family.HeadOfFamilyId == person.Id)
            {
                return;
            }

            family.HeadOfFamilyId = person.Id;
        }
    }
}
