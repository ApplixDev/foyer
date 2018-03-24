using Abp.Domain.Repositories;
using Abp.Domain.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Foyer.People
{
    public class PersonManager : DomainService, IPersonManager
    {
        private IRepository<Person> _personRepository;

        public PersonManager(IRepository<Person> personRepository)
        {
            _personRepository = personRepository;
        }

        public bool PersonExists(Person person)
        {
            return _personRepository.GetAll().Any
            (
                p => p.FirstName == person.FirstName &&
                p.LastName == person.LastName &&
                p.BirthDate == person.BirthDate
            );
        }
    }
}
