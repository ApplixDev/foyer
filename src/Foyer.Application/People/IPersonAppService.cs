using Abp.Application.Services;
using Foyer.People.Dto;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Foyer.People
{
    public interface IPersonAppService : IApplicationService
    {
        IEnumerable<GetPersonOutput> ListAll();
        Task CreatePersonAsync(CreatePersonInput input);
        void UpdatePerson(UpdatePersonInput input);
        void DeletePerson(DeletePersonInput input);
        GetPersonOutput GetPersonById(GetPersonInput input);
    }
}