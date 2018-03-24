using Abp.Domain.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Foyer.People
{
    public interface IPersonManager : IDomainService
    {
        bool PersonExists(Person person);
    }
}
