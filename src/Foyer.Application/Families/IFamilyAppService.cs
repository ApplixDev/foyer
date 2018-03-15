using Abp.Application.Services;
using Foyer.Families.Dto;
using System.Threading.Tasks;

namespace Foyer.Families
{
    public interface IFamilyAppService : IApplicationService
    {
        void Create(CreateFamilyDto input);
        void Update(UpdateFamilyDto input);
        void Delete(DeleteFamilyInput input);
        FamilyDto Get(GetFamilyInput input);
        void AssignFamilyParents(AssignFamilyParentsInput input);
        void AssignFamilyFather(AssignFamilyParentInput input);
        void AssignFamilyMother(AssignFamilyParentInput input);
        Task<GetAllFamiliesOutput> GetAllFamilies();
        //Task<GetFamiliesWithParentsOutput> GetAllFamiliesWithParents();// Is it useful ?
        //Task<GetFamiliesWithMembersOutput> GetAllFamiliesWithMembers();
        //Task<GetFamilyMembersOutput> GetAllFamilyMembers(GetFamilyMembersInput input);
        //void AddFamilyMember(AddFamilyMemberInput input);
        //void UpdateFamilyMember(UpdateFamilyMemberInput input);
        //void DeleteFamilyMember(DeleteFamilyMemberInput input);
    }
}
