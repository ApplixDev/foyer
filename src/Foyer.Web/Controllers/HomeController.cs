using System;
using System.Linq;
using System.Web.Mvc;
using Abp.Collections.Extensions;
using Abp.Domain.Repositories;
using Abp.Web.Mvc.Authorization;
using Foyer.Families;
using Foyer.People;

namespace Foyer.Web.Controllers
{
    [AbpMvcAuthorize]
    public class HomeController : FoyerControllerBase
    {
        private readonly IRepository<Family> _familyRepository;
        private readonly IRepository<Person> _personRepository;

        private Family newFamily;

        public HomeController(IRepository<Person> personRepository, IRepository<Family> familyRepository)
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

            var newFamily = _familyRepository.InsertOrUpdate(new Family
            {
                FatherId = 1,
                MotherId = 2
            });
        }

        public ActionResult Index()
        {
            var familyExist = _familyRepository.GetAll()
                .WhereIf(newFamily.FatherId.HasValue, f => f.FatherId == newFamily.FatherId)
                .WhereIf(!newFamily.FatherId.HasValue, f => f.FatherId == null)
                .WhereIf(newFamily.MotherId.HasValue, f => f.MotherId == newFamily.MotherId)
                .WhereIf(!newFamily.MotherId.HasValue, f => f.MotherId == null)
                .Any();

            //.Where(f => f.FatherId == inputFamily.FatherId.HasValue ? inputFamily.FatherId : null)
            //.Where(f => f.MotherId == inputFamily.MotherId.HasValue ? inputFamily.MotherId : null)
            //.Any();

            return View();
        }
	}
}