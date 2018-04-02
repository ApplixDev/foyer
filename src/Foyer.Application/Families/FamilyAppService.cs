using System;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using Abp.ObjectMapping;
using Abp.UI;
using Foyer.Families.Dto;
using Foyer.FamilyRelationships;
using Foyer.People;
using Abp.Application.Services;
using System.Collections.Generic;

namespace Foyer.Families
{
    public class FamilyAppService : FoyerAppServiceBase, IFamilyAppService
    {
        private readonly IRepository<Person> _personRepository;
        private readonly IRepository<Family> _familyRepository;
        private readonly IRepository<FamilyRelationship> _familyRelationshipsRepository;
        private readonly IFamilyManager _familyManager;
        private readonly IObjectMapper _objectMapper;
        private readonly IUnitOfWorkManager _unitOfWorkManager;

        public FamilyAppService(
            IRepository<Person> personRepository,
            IRepository<Family> familyRepository,
            IRepository<FamilyRelationship> familyRelationshipsRepository,
            IFamilyManager familyManager,
            IObjectMapper objectMapper,
            IUnitOfWorkManager unitOfWorkManager)
        {
            _personRepository = personRepository;
            _familyRepository = familyRepository;
            _familyRelationshipsRepository = familyRelationshipsRepository;
            _familyManager = familyManager;
            _objectMapper = objectMapper;
            _unitOfWorkManager = unitOfWorkManager;
        }

        public void Create(CreateFamilyDto inputFamily)
        {
            var family = MapToEntity(inputFamily);

            if (_familyManager.ParentsFamilyExists(family))
            {
                throw new UserFriendlyException(L("FamilyAlreadyExists"));
            }

            GetAndAssignFamilyParents(family);

            _familyRepository.Insert(family);
        }

        private void GetAndAssignFamilyParents(Family family)
        {
            if (family.FatherId.HasValue)
            {
                var father = _personRepository.Get(family.FatherId.Value);
                _familyManager.AssignFamilyFather(family, father);
            }

            if (family.MotherId.HasValue)
            {
                var mother = _personRepository.Get(family.MotherId.Value);
                _familyManager.AssignFamilyMother(family, mother);
            }
        }

        public void Update(UpdateFamilyDto input)
        {
            var family = _familyRepository.Get(input.FamilyId);
            MapToEntity(input, family);
        }

        public void Delete(DeleteFamilyInput input)
        {
            var family = _familyRepository.FirstOrDefault(p => p.Id == input.FamilyId);

            if (family == null)
            {
                throw new UserFriendlyException(L("FamilyDoesNotExists"));
            }

            _familyRepository.Delete(input.FamilyId);
        }

        public FamilyDto Get(GetFamilyInput input)
        {
            var family = _familyRepository.Get(input.FamilyId);
            return _objectMapper.Map<FamilyDto>(family);
        }

        public async Task<GetAllFamiliesOutput> GetAllFamilies()
        {
            var families = await _familyRepository.GetAllListAsync();

            return new GetAllFamiliesOutput
            {
                Families =  _objectMapper.Map<List<FamilyDto>>(families)
            };
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
