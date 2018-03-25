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
        /// <summary>
        /// Check if any person exists,
        /// full name and birthdate are used to find matching person.
        /// </summary>
        /// <param name="person"></param>
        /// <returns></returns>
        bool PersonExists(Person person);
    }
}
