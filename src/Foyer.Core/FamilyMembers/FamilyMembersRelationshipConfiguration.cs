using Foyer.FamilyMembers;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Foyer.EntityFramework
{
    class FamilyMembersRelationshipConfiguration : EntityTypeConfiguration<FamilyMembersRelationship>
    {
        public FamilyMembersRelationshipConfiguration()
        {
            this.HasRequired(r => r.Person)
                .WithMany(p => p.RelationshipsAsFirstMember)
                .HasForeignKey(r => r.PersonId)
                .WillCascadeOnDelete(false);

            this.HasRequired(r => r.RelatedPerson)
                .WithMany(p => p.RelationshipsAsSecondMember)
                .HasForeignKey(r => r.RelatedPersonId)
                .WillCascadeOnDelete(false);
        }
    }
}
