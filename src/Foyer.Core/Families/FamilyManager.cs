using System;
using System.Linq;
using Abp.Domain.Repositories;
using Abp.Domain.Services;
using Abp.Localization;
using Abp.UI;
using Foyer.People;

namespace Foyer.Families
{
    public class FamilyManager : FoyerDomainServiceBase, IFamilyManager
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
                throw new UserFriendlyException(L("FamilyFatherMustBeMale"));
            }
            
            if (father.IsTransient())
            {
                throw new ApplicationException("Assign transient person as family parent is not allowed, person id is required");
            }

            family.FatherId = father.Id;
        }

        public void AssignFamilyMother(Family family, Person mother)
        {
            if (mother.Gender != Gender.Female)
            {
                throw new UserFriendlyException(L("FamilyMotherMustBeFemale"));
            }

            if (mother.IsTransient())
            {
                throw new ApplicationException("Assign transient person as family parent is not allowed, person id is required");
            }
                
            family.MotherId = mother.Id;
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
