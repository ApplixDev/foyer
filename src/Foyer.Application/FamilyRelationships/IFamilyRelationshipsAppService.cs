using Abp.Application.Services;
using Foyer.Families;
using Foyer.People;

namespace Foyer.FamilyRelationships
{
    public interface IFamilyRelationshipsAppService : IApplicationService
    {
        void AddFamilyMember(Family family, Person person, FamilyRelationship relationship);
        void DeleteFamilyMember(Family family, Person person);
        void AddRelationship(FamilyRelationship relationship);
        void AddOrUpdateParentsRelationship(Family family, bool married);
    }
}
