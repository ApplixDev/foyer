using Abp.Domain.Repositories;
using Abp.ObjectMapping;
using Abp.UI;
using Foyer.People.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Foyer.People
{
    public class PersonAppService : IPersonAppService
    {
        private IRepository<Person> _personRepository;
        private IObjectMapper _objectMapper;

        public PersonAppService(IRepository<Person> personRepository, IObjectMapper objectMapper)
        {
            _personRepository = personRepository;
            _objectMapper = objectMapper;
        }

        public void Create(CreatePersonDto input)
        {
            CheckIfInputPersonExist(input);
            var person = MapToEntity(input);
            _personRepository.Insert(person);
        }

        public async Task CreateAsync(CreatePersonDto input)
        {
            CheckIfInputPersonExist(input);
            var person = MapToEntity(input);
            await _personRepository.InsertAsync(person);
        }

        private void CheckIfInputPersonExist(CreatePersonDto input)
        {
            var person = _personRepository.FirstOrDefault
            (
                p => p.FirstName == input.FirstName &&
                p.LastName == input.LastName &&
                p.BirthDate == input.BirthDate
            );

            if (person != null)
            {
                throw new UserFriendlyException("This Person already exist");
            }
        }

        public void Update(UpdatePersonDto input)
        {
            Person person = _personRepository.Get(input.PersonId);
            MapToEntity(input, person);
        }

        public void Delete(DeletePersonInput input)
        {
            var person = _personRepository.FirstOrDefault(p => p.Id == input.PersonId);

            if (person == null)
            {
                throw new UserFriendlyException("This Person does not exist");
            }

            _personRepository.Delete(input.PersonId);
        }

        public PersonDto Get(GetPersonInput input)
        {
            var person = _personRepository.Get(input.PersonId);
            return _objectMapper.Map<PersonDto>(person);
        }

        public async Task<GetAllPeopleOutput> GetAllPeople()
        {
            var people = await _personRepository.GetAllListAsync();
            
            return new GetAllPeopleOutput
            {
                People = _objectMapper.Map<List<PersonDto>>(people)
            };
        }

        protected Person MapToEntity(CreatePersonDto input)
        {
            return _objectMapper.Map<Person>(input);
        }

        protected void MapToEntity(UpdatePersonDto input, Person person)
        {
            _objectMapper.Map(input, person);
        }
    }
}
