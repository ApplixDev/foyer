using Abp.Domain.Services;
using Abp.UI;
using Foyer.People;

namespace Foyer.Families
{
    public class FamilyManager : DomainService, IFamilyManager
    {
        public void AssignFamilyParents(Family family, Person father, Person mother)
        {
            AssignFamilyFather(family, father);
            AssignFamilyMother(family, mother);
        }

        public void AssignFamilyFather(Family family, Person father)
        {
            if (family.FatherId == father.Id)
            {
                return;
            }

            if (father.Gender != Gender.Male)
            {
                throw new UserFriendlyException("The family father must be a male");
            }

            family.FatherId = father.Id;
        }

        public void AssignFamilyMother(Family family, Person mother)
        {
            if (family.MotherId == mother.Id)
            {
                return;
            }

            if (mother.Gender != Gender.Female)
            {
                throw new UserFriendlyException("The family mother must be a female");
            }

            family.MotherId = mother.Id;
        }
    }
}
