using Abp.Application.Services;
using Foyer.People.Dto;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Foyer.People
{
    public interface IPersonAppService : IApplicationService
    {
        Task<GetAllPeopleOutput> GetAllPeople();
        void Create(CreatePersonDto input);
        Task CreateAsync(CreatePersonDto input);
        void Update(UpdatePersonDto input);
        void Delete(DeletePersonInput input);
        PersonDto Get(GetPersonInput input);
    }
}