using Foyer.FamilyRelationships;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Foyer.EntityFramework
{
    class FamilyRelationshipConfiguration : EntityTypeConfiguration<FamilyRelationship>
    {
        public FamilyRelationshipConfiguration()
        {
            this.HasRequired(r => r.Family)
                .WithMany(f => f.FamilyRelationships)
                .HasForeignKey(r => r.FamilyId);

            this.HasRequired(r => r.Person)
                .WithMany(p => p.RelationshipsAsPrincipalPerson)
                .HasForeignKey(r => r.PersonId)
                .WillCascadeOnDelete(false);

            this.HasRequired(r => r.RelatedPerson)
                .WithMany(p => p.RelationshipsAsRelatedPerson)
                .HasForeignKey(r => r.RelatedPersonId)
                .WillCascadeOnDelete(false);
        }
    }
}
