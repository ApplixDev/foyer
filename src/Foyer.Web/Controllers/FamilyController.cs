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

        private Family family;

        public FamilyController(IRepository<Family> familyRepository)
        {
            _familyRepository = familyRepository;

            family = _familyRepository.Get(2);
        }

        public void Index()
        {
            var familyExist = _familyRepository.GetAll()
                .Any(f => f.FatherId == family.FatherId && f.MotherId == family.MotherId);
        }
    }
}