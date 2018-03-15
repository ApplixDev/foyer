using Abp.Application.Services;
using Foyer.Families;
using Foyer.People;

namespace Foyer.FamilyRelationships
{
    public interface IFamilyRelationshipsAppService : IApplicationService
    {
        //void AddFamilyRelationship();//Add a relationship is adding a member
        void AddFamilyMember(Family family, Person person, FamilyRelationship relationship);
        void DeleteFamilyMember(Family family, Person person);
        void AddFamilyRelationship(FamilyRelationship relationship);
    }
}
