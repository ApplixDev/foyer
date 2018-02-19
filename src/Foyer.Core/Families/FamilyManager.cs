using Abp.Domain.Services;
using Foyer.People;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Foyer.Families
{
    class FamilyManager : DomainService, IFamilyManager
    {
        public FamilyManager()
        {

        }

        public void AddPersonToFamily(Family family, Person person)
        {
            throw new NotImplementedException();
        }

        public void AssignPersonHeadOfFamily(Family family, Person person)
        {
            throw new NotImplementedException();
        }
    }
}
