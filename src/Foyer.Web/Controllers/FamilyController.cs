using Abp.Collections.Extensions;
using Abp.Domain.Repositories;
using Foyer.Families;
using Foyer.People;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Foyer.Web.Controllers
{
    public class FamilyController : FoyerControllerBase
    {
        private readonly IRepository<Family> _familyRepository;
        private readonly IRepository<Person> _personRepository;

        private Family newFamily;

        public FamilyController(IRepository<Person> personRepository, IRepository<Family> familyRepository)
        {
            _personRepository = personRepository;
            _familyRepository = familyRepository;

            var father = _personRepository.Insert(new Person
            {
                FirstName = "Lo",
                LastName = "Celso",
                Gender = Gender.Male,
                BirthDate = new DateTime(2000, 1, 1)
            });

            var mother = _personRepository.Insert(new Person
            {
                FirstName = "Lo",
                LastName = "Salca",
                Gender = Gender.Female,
                BirthDate = new DateTime(2000, 1, 1)
            });

            newFamily = _familyRepository.Insert(new Family
            {
                FatherId = father.Id,
                MotherId = mother.Id
            });
        }

        public void Index()
        {
            var familyExist = _familyRepository.GetAll()
                .WhereIf(newFamily.FatherId.HasValue, f => f.FatherId == newFamily.FatherId);

            if (familyExist.Any())
            {
                var exist = "Exist";
                exist = true.ToString();
            }

            var query1 = _familyRepository.GetAll();

            if (newFamily.FatherId.HasValue)
            {
                query1 = query1.Where(f => f.FatherId == newFamily.FatherId);
            }
            else
            {
                query1 = query1.Where(f =>f.FatherId == null);
            }

            if (newFamily.MotherId.HasValue)
            {
                query1 = query1.Where(f => f.MotherId == newFamily.MotherId);
            }
            else
            {
                query1 = query1.Where(f => f.MotherId == null);
            }

            if (query1.Any())
            {
                var exist = "Exist";
                exist = true.ToString();
            }

            var query2 = _familyRepository.GetAll();

            var fatherId = newFamily.FatherId.HasValue ? newFamily.FatherId : null;
            var motherId = newFamily.MotherId.HasValue ? newFamily.MotherId : null;

            query2 = query2
                .Where(f => f.FatherId == fatherId)
                .Where(f => f.MotherId == motherId);


            if (query2.Any())
            {
                var exist = "Exist";
                exist = true.ToString();
            }

            var query3 = _familyRepository.GetAll();

            query3 = query3
                .Where(f => f.FatherId == (newFamily.FatherId.HasValue ? newFamily.FatherId : null))
                .Where(f => f.MotherId == (newFamily.MotherId.HasValue ? newFamily.MotherId : null));

            if (query3.Any())
            {
                var exist = "Exist";
                exist = true.ToString();
            }

            var query4 = _familyRepository.GetAll()
                .Where(f => f.FatherId == newFamily.FatherId && f.MotherId == newFamily.MotherId);

            if (query4.Any())
            {
                var exist = "Exist";
                exist = true.ToString();
            }
        }
    }
}