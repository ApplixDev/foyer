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
        private IPersonManager _personManager;
        private IObjectMapper _objectMapper;

        public PersonAppService(IRepository<Person> personRepository, IPersonManager personManager, IObjectMapper objectMapper)
        {
            _personRepository = personRepository;
            _personManager = personManager;
            _objectMapper = objectMapper;
        }

        public void Create(CreatePersonDto input)
        {
            var person = MapToEntity(input);

            if (_personManager.PersonExists(person))
            {
                throw new UserFriendlyException("This Person already exist");
            }

            _personRepository.Insert(person);
        }

        public async Task CreateAsync(CreatePersonDto input)
        {
            var person = MapToEntity(input);

            if (_personManager.PersonExists(person))
            {
                throw new UserFriendlyException("This Person already exist");
            }

            await _personRepository.InsertAsync(person);
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
