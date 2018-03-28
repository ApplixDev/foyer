using System.Linq;
using Abp.Domain.Repositories;
using Abp.Domain.Services;
using Abp.UI;
using Foyer.People;

namespace Foyer.Families
{
    public class FamilyManager : DomainService, IFamilyManager
    {
        private readonly IRepository<Family> _familyRepository;

        public FamilyManager(IRepository<Family> familyRepository)
        {
            _familyRepository = familyRepository;
        }

        public void AssignFamilyParents(Family family, Person father, Person mother)
        {
            AssignFamilyFather(family, father);
            AssignFamilyMother(family, mother);
        }

        public void AssignFamilyFather(Family family, Person father)
        {
            if (father.Gender != Gender.Male)
            {
                throw new UserFriendlyException("The family father must be a male");
            }

            if (family.FatherId == father.Id)//if (family.Father == father) //To search and test
            {
                return;
            }

            family.FatherId = father.Id;
            //family.Father = father;
        }

        public void AssignFamilyMother(Family family, Person mother)
        {
            if (mother.Gender != Gender.Female)
            {
                throw new UserFriendlyException("The family mother must be a female");
            }

            if (family.MotherId == mother.Id)//if (family.Mother == mother) //To search and test
            {
                return;
            }

            family.MotherId = mother.Id;
            //family.Mother = mother;
        }

        public bool ParentsFamilyExists(Family family)
        {
            return _familyRepository.GetAll().Any
            (
                f => f.FatherId == family.FatherId &&
                f.MotherId == family.MotherId
            );
        }
    }
}
