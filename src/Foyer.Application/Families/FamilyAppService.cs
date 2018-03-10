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

        public void Create(CreateFamilyDto input)
        {
            CheckIfInputFamilyExist(input);

            var family = MapToEntity(input);

            _familyRepository.Insert(family);
        }

        private void CheckIfInputFamilyExist(CreateFamilyDto input)
        {
            var familyExist = _familyRepository.GetAll()
                .WhereIf(input.HusbandId.HasValue, f => f.HusbandId == input.HusbandId)
                .WhereIf(!input.HusbandId.HasValue, f => f.HusbandId == null)
                .WhereIf(input.WifeId.HasValue, f => f.WifeId == input.WifeId)
                .WhereIf(!input.WifeId.HasValue, f => f.WifeId == null)
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

        public void AssignFamilyFather(AssignFamilyParentInput input)
        {
            var father = _personRepository.Get(input.ParentId);

            if (father.Gender == Gender.Female)
            {
                throw new UserFriendlyException("You can not assign a female as a family father, a father must be a male");
            }

            var family = _familyRepository.Get(input.FamilyId);

            _familyManager.AssignFamilyFather(family, father);
        }

        public void AssignFamilyMother(AssignFamilyParentInput input)
        {
            var mother = _personRepository.Get(input.ParentId);

            if (mother.Gender == Gender.Male)
            {
                throw new UserFriendlyException("You can not assign a male as a family mother, a mother must be a female");
            }

            var family = _familyRepository.Get(input.FamilyId);

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
