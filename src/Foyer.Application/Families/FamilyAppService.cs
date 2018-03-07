using System;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using Abp.ObjectMapping;
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

        public void Create(CreateFamilyInput input)
        {
            throw new NotImplementedException();
        }

        public void Update(UpdateFamilyInput input)
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

        public void AssignPersonHeadOfFamily(PersonHeadOfFamilyInput input)
        {
            var person = _personRepository.Get(input.PersonId);
            var family = _familyRepository.Get(input.FamilyId);

            _familyManager.AssignPersonHeadOfFamily(person, family);
        }
    }
}
