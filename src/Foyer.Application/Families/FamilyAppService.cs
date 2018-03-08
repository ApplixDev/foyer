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

        public void Create(CreateFamilyInput input)
        {
            //Try find methode to identify existing family:

            //var family = _familyRepository.FirstOrDefault(
            //                f => f.FamilyName != null &&
            //                f.FamilyName == input.FamilyName &&
            //                f.HeadOfFamilyId != null &&
            //                f.HeadOfFamilyId == input.HeadOfFamilyId &&
            //                f.WidingDate != null &&
            //                f.WidingDate == input.WidingDate);

            if (input.FamilyName != null && input.HeadOfFamilyId.HasValue && input.WidingDate != null)
            {
                var family = _familyRepository.FirstOrDefault(
                            f => f.FamilyName == input.FamilyName &&
                            f.HeadOfFamilyId == input.HeadOfFamilyId &&
                            f.WidingDate == input.WidingDate);

                if (family != null)
                {
                    throw new UserFriendlyException("This family already exist");
                }
            }

            if (family != null)
            {
                return;
            }

            family = _objectMapper.Map<Family>(input);

            _familyRepository.Insert(family);
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
