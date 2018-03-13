using System;
using System.Linq;
using System.Threading.Tasks;
using Abp.Collections.Extensions;
using Abp.Domain.Repositories;
using Abp.ObjectMapping;
using Abp.UI;
using Foyer.Families.Dto;
using Foyer.People;

namespace Foyer.Families
{
    public class FamilyAppService : IFamilyAppService
    {
        private readonly IRepository<Person> _personRepository;
        private readonly IRepository<Family> _familyRepository;
        private readonly IFamilyManager _familyManager;
        private readonly IObjectMapper _objectMapper;

        public FamilyAppService(
            IRepository<Person> personRepository,
            IRepository<Family> familyRepository,
            IFamilyManager familyManager,
            IObjectMapper objectMapper)
        {
            _personRepository = personRepository;
            _familyRepository = familyRepository;
            _familyManager = familyManager;
            _objectMapper = objectMapper;
        }

        public void Create(CreateFamilyDto inputFamily)
        {
            CheckIfFamilyExist(inputFamily);

            var family = MapToEntity(inputFamily);

            if (inputFamily.FatherId.HasValue && inputFamily.MotherId.HasValue)
            {
                //Should add new marriage relationship if not exist ?
                //_familyRelationshipManager.AddRelationship
            }

            if (inputFamily.FatherId.HasValue)
            {
                var father = _personRepository.Get(inputFamily.FatherId.Value);
                _familyManager.AssignFamilyFather(family, father);
                //family.Father = father;//To test
            }

            if(inputFamily.MotherId.HasValue)
            {
                var mother = _personRepository.Get(inputFamily.MotherId.Value);
                _familyManager.AssignFamilyMother(family, mother);
                //family.Mother = mother;//To test
            }

            _familyRepository.Insert(family);
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
