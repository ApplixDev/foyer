using Abp.Application.Services;
using Foyer.Families.Dto;
using Foyer.People;
using System.Threading.Tasks;

namespace Foyer.Families
{
    public interface IFamilyAppService : IApplicationService
    {
        Task<GetAllFamiliesOutput> GetAllFamilies();
        //Task<FamiliesWithParentsOutput> GetAllFamiliesIncludingParents();
        void Create(CreateFamilyDto input);
        void Update(UpdateFamilyDto input);
        void Delete(DeleteFamilyInput input);
        FamilyDto Get(GetFamilyInput input);
        void AssignFamilyParents(AssignFamilyParentsInput input);
        void AssignFamilyFather(AssignFamilyParentInput input);
        void AssignFamilyMother(AssignFamilyParentInput input);
    }
}
