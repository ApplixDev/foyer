using Abp.Domain.Services;
using Foyer.People;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Foyer.Families
{
    interface IFamilyManager : IDomainService
    {
        void AddPersonToFamily(Family family, Person person);
        void AssignPersonHeadOfFamily(Family family, Person person);
    }
}
