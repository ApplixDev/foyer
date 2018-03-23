﻿using System;
using System.Linq;
using System.Threading.Tasks;
using Abp.Collections.Extensions;
using Abp.Domain.Repositories;
using Abp.Domain.Uow;
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
            ThrowExceptionIfFamilyExists(inputFamily.FatherId, inputFamily.MotherId);

            var family = MapToEntity(inputFamily);

            GetAndAssignFamilyParents(family);

            _familyRepository.Insert(family);
        }

        private void ThrowExceptionIfFamilyExists(int? fatherId, int? motherId)
        {
            var familyExists = _familyRepository.GetAll()
                .Any(f => f.FatherId == fatherId && f.MotherId == motherId);

            if (familyExists)
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

        public void Update(UpdateFamilyDto input)
        {
            
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
