using System;
using System.Linq;
using System.Threading.Tasks;
using Abp.Collections.Extensions;
using Abp.Domain.Repositories;
using Abp.ObjectMapping;
using Abp.UI;
using Foyer.Families.Dto;
using Foyer.FamilyRelationships;
using Foyer.People;

namespace Foyer.Families
{
    public class FamilyAppService : IFamilyAppService
    {
        private readonly IRepository<Person> _personRepository;
        private readonly IRepository<Family> _familyRepository;
        private readonly IRepository<FamilyRelationship> _familyRelationshipsRepository;
        private readonly IFamilyManager _familyManager;
        private readonly IObjectMapper _objectMapper;

        public FamilyAppService(
            IRepository<Person> personRepository,
            IRepository<Family> familyRepository,
            IRepository<FamilyRelationship> familyRelationshipsRepository,
            IFamilyManager familyManager,
            IObjectMapper objectMapper)
        {
            _personRepository = personRepository;
            _familyRepository = familyRepository;
            _familyRelationshipsRepository = familyRelationshipsRepository;
            _familyManager = familyManager;
            _objectMapper = objectMapper;
        }

        public void Create(CreateFamilyDto inputFamily)
        {
            CheckIfFamilyExist(inputFamily);

            var family = MapToEntity(inputFamily);

            GetAndAssignFamilyParents(family);

            _familyRepository.Insert(family);

            if (family.FatherId.HasValue && family.MotherId.HasValue)
            {
                AddMarriageRelationshipIfNotExist(family);
            }
        }

        private void CheckIfFamilyExist(CreateFamilyDto inputFamily)
        {
            var familyExist = _familyRepository.GetAll()
                .WhereIf(inputFamily.FatherId.HasValue, f => f.FatherId == inputFamily.FatherId)
                .WhereIf(!inputFamily.FatherId.HasValue, f => f.FatherId == null)
                .WhereIf(inputFamily.MotherId.HasValue, f => f.MotherId == inputFamily.MotherId)
                .WhereIf(!inputFamily.MotherId.HasValue, f => f.MotherId == null)
                .Any();

            if (familyExist)
            {
                throw new UserFriendlyException("This family already exist");
            }
        }

        private void GetAndAssignFamilyParents(Family family)
        {
            if (family.FatherId.HasValue)
            {
                var father = _personRepository.Get(family.FatherId.Value);
                _familyManager.AssignFamilyFather(family, father);
                //family.Father = father;//To test or to move to FamilyManager
            }

            if (family.MotherId.HasValue)
            {
                var mother = _personRepository.Get(family.MotherId.Value);
                _familyManager.AssignFamilyMother(family, mother);
                //family.Mother = mother;//To test or to move to FamilyManager
            }
        }

        private void AddMarriageRelationshipIfNotExist(Family family)
        {
            var relationship = _familyRelationshipsRepository.FirstOrDefault
            (
                r => r.PersonId == family.FatherId
                && r.RelatedPersonId == family.MotherId
                && r.RelationshipType == RelationshipType.Married
            );

            if (relationship != null)
            {
                return;
            }

            _familyRelationshipsRepository.Insert(new FamilyRelationship
            {
                RelationshipType = RelationshipType.Married,
                FamilyId = family.Id,
                PersonId = family.FatherId.Value,
                PersonRole = PersonRole.Husband,
                RelatedPersonId = family.MotherId.Value,
                RelatedPersonRole = PersonRole.Wife
            });
        }

        public void Update(UpdateFamilyDto input)
        {
            throw new NotImplementedException();
        }

        public void Delete(DeleteFamilyInput input)
        {
            throw new NotImplementedException();
        }

        public FamilyDto Get(GetFamilyInput input)
        {
            throw new NotImplementedException();
        }

        public Task<GetAllFamiliesOutput> GetAllFamilies()
        {
            throw new NotImplementedException();
        }

        public void AssignFamilyParents(AssignFamilyParentsInput input)
        {
            var family = _familyRepository.Get(input.FamilyId);
            var father = _personRepository.Get(input.FatherId);
            var mother = _personRepository.Get(input.MotherId);

            _familyManager.AssignFamilyParents(family, father, mother);
        }

        public void AssignFamilyFather(AssignFamilyParentInput input)
        {
            var family = _familyRepository.Get(input.FamilyId);
            var father = _personRepository.Get(input.ParentId);

            _familyManager.AssignFamilyFather(family, father);
        }

        public void AssignFamilyMother(AssignFamilyParentInput input)
        {
            var family = _familyRepository.Get(input.FamilyId);
            var mother = _personRepository.Get(input.ParentId);

            _familyManager.AssignFamilyMother(family, mother);
        }

        protected Family MapToEntity(CreateFamilyDto input)
        {
            return _objectMapper.Map<Family>(input);
        }

        protected void MapToEntity(UpdateFamilyDto input, Family family)
        {
            _objectMapper.Map(input, family);
        }
    }
}
