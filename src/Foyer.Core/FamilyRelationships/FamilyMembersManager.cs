using Abp.Domain.Services;
using Foyer.Families;
using Foyer.People;
using System;

namespace Foyer.FamilyRelationships
{
    public class FamilyMembersManager : DomainService, IFamilyMembersManager
    {
        public FamilyMembersManager()
        {

        }

        public void AddFamilyMember(Family family, Person person)
        {
            // There is many cases:
            //
            // 1- Create new relationship between existing person and family (need relationship informations).
            //
            // 2- Find and update a relationship between two person (default head of family) and set family id
            // because a relationship between two person can persiste without a defined family (family should be required ?).

            if (family != null && person != null)
            {
                var relationship = new FamilyRelationship
                {
                   Family = family,
                   Person = person,
                   
                };

                //return relationship;
            }

            //throw user friendly exception
        }

        public void DeleteFamilyMember(Family family, Person person)
        {
            throw new NotImplementedException("Should delete all relationships between this person and all members of this family including the head of family");
        }

        // Should be deleted ? because of it should be a stateless domain service methode.
        public void AddFamilyMembersRelationship(FamilyRelationship relationship)
        {
            throw new NotImplementedException();
        }
    }
}
