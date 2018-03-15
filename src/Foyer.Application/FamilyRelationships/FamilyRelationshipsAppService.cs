using Foyer.Families;
using Foyer.People;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Foyer.FamilyRelationships
{
    public class FamilyRelationshipsAppService : IFamilyRelationshipsAppService
    {
        public void AddFamilyMember(Family family, Person member, FamilyRelationship relationshipWithParent)
        {
            //If the person is a parent, call AssignFamilyParents of app service because it will set parents in family entity ?

            //Find and update a relationship between the person and the parents,
            //first with thz father then with the mother

            //If not exist create new relationship between the person and family parents.

            if (family != null && member != null)
            {
                var relationship = new FamilyRelationship
                {
                    Family = family,
                    Person = member,

                };

                //return relationship;
            }

            //throw user friendly exception
        }

        public void DeleteFamilyMember(Family family, Person member)
        {
            //Delete all relationships Where(r.FamilyId == family.Id && (r.PersonId == member.Id || r.RelatedPersonId == member.Id)); 
            throw new NotImplementedException("Should delete all relationships between this person and all members of this family including the parents");
        }

        // Should be deleted ? because it should be a stateless domain service methode.
        public void AddFamilyRelationship(FamilyRelationship relationship)
        {
            throw new NotImplementedException();
        }
    }
}
