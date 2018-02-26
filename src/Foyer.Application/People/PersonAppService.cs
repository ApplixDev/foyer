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

        public void CreatePerson(CreatePersonInput input)
        {
            var person = CheckAndMapInputToPerson(input);
            _personRepository.Insert(person);
        }

        public async Task CreatePersonAsync(CreatePersonInput input)
        {
            var person = CheckAndMapInputToPerson(input);
            await _personRepository.InsertAsync(person);
        }

        private Person CheckAndMapInputToPerson(CreatePersonInput input)
        {
            var person = _personRepository.FirstOrDefault(
                            p => p.FirstName == input.FirstName &&
                            p.LastName == input.LastName &&
                            p.DateOfBirth == input.DateOfBirth);

            if (person != null)
            {
                throw new UserFriendlyException("This Person already exist");
            }

            return _objectMapper.Map<Person>(input);
        }

        public void DeletePerson(DeletePersonInput input)
        {
            _personRepository.Delete(input.PersonId);
        }

        public GetPersonOutput GetPersonById(GetPersonInput input)
        {
            var person = _personRepository.Get(input.PersonId);
            return _objectMapper.Map<GetPersonOutput>(person);
        }

        public IEnumerable<GetPersonOutput> ListAll()
        {
            var people = _personRepository.GetAllList().ToList();
            return _objectMapper.Map<List<GetPersonOutput>>(people);
        }

        public void UpdatePerson(UpdatePersonInput input)
        {
            Person person = _objectMapper.Map<Person>(input);
            _personRepository.Update(person);
        }
    }
}
