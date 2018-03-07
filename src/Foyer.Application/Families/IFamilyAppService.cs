using Abp.Application.Services;
using Foyer.Families.Dto;
using Foyer.People;
using System.Threading.Tasks;

namespace Foyer.Families
{
    public interface IFamilyAppService : IApplicationService
    {
        Task<GetAllFamiliesOutput> GetAllFamilies();
        void Create(CreateFamilyInput input);
        void Update(UpdateFamilyInput input);
        void Delete(DeleteFamilyInput input);
        FamilyDto Get(GetFamilyInput input);
        void AssignPersonHeadOfFamily(PersonHeadOfFamilyInput input);
    }
}
