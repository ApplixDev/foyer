﻿using Abp.Domain.Entities;
using Foyer.Families;
using Foyer.People;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Foyer.FamilyRelationships
{
    /// <summary>
    /// Represents the relationship between two people whithin a single family.
    /// </summary>
    public class FamilyMembersRelationship : Entity, ISoftDelete
    {
        public const int MaxDetailsLength = 64 * 1024; //64KB

        /// <summary>
        /// Family concerned by the relationship.
        /// </summary>
        [ForeignKey(nameof(FamilyId))]
        public virtual Family Family { get; set; }
        public virtual int FamilyId { get; set; }

        /// <summary>
        /// First person concerned by the relationship.
        /// </summary>
        //[ForeignKey(nameof(PersonId))]
        public virtual Person Person { get; set; }
        public virtual int PersonId { get; set; }

        /// <summary>
        /// Second person concerned by the relationship.
        /// </summary>
        //[ForeignKey(nameof(RelatedPersonId))]
        public virtual Person RelatedPerson { get; set; }
        public virtual int RelatedPersonId { get; set; }

        /// <summary>
        /// Relationship type.
        /// </summary>
        public virtual RelationshipType RelationshipType { get; set; }

        /// <summary>
        /// First person role in the relationship.
        /// </summary>
        public virtual RelationshipRole PersonRole { get; set; }

        /// <summary>
        /// Second person role in the relationship.
        /// </summary>
        public virtual RelationshipRole RelatedPersonRole { get; set; }

        /// <summary>
        /// Other details about the relationship.
        /// </summary>
        [StringLength(MaxDetailsLength)]
        public virtual string OtherDetails { get; set; }

        /// <summary>
        /// Mark a relationship as deleted.
        /// </summary>
        public bool IsDeleted { get; set; }
    }

}
