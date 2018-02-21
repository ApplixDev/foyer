using Abp.Domain.Services;
using Foyer.Families;
using Foyer.People;

namespace Foyer.FamilyMembers
{
    public interface IFamilyMembersManager : IDomainService
    {
        void AddFamilyMember(Family family, Person person);
        void DeleteFamilyMember(Family family, Person person);
        // Should be deleted ? because of it should be a stateless domain service methode.
        void AddFamilyMembersRelationship(FamilyMembersRelationship relationship);
    }
}
