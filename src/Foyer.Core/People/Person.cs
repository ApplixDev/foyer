using Abp.Domain.Entities;
using Foyer.Families;
using Foyer.FamilyMembers;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Foyer.People
{
    public class Person : Entity, ISoftDelete
    {
        public const int MaxDetailsLength = 64 * 1024; //64KB
        public const int MaxNameLength = 32;

        /// <summary>
        /// First name.
        /// </summary>
        [Required]
        [StringLength(MaxNameLength)]
        public virtual string FirstName { get; set; }

        /// <summary>
        /// Last name.
        /// </summary>
        [Required]
        [StringLength(MaxNameLength)]
        public virtual string LastName { get; set; }

        /// <summary>
        /// Male or female.
        /// </summary>
        [Required]
        public virtual Gender Gender { get; set; }

        /// <summary>
        /// Date of birth.
        /// </summary>
        public virtual DateTime? DateOfBirth { get; set; }

        /// <summary>
        /// Place of birth.
        /// </summary>
        public virtual DateTime? PlaceOfBirth { get; set; }

        /// <summary>
        /// Other individual details.
        /// </summary>
        [StringLength(MaxDetailsLength)]
        public virtual string OtherDetails { get; set; }

        /// <summary>
        /// Used to mark individual as deleted.
        /// </summary>
        public virtual bool IsDeleted { get; set; }

        /// <summary>
        /// Family members relationships.
        /// </summary>
        public virtual ICollection<FamilyMembersRelationship> RelationshipsAsFirstMember { get; set; } = new HashSet<FamilyMembersRelationship>();

        /// <summary>
        /// Family members relationships.
        /// </summary>
        public virtual ICollection<FamilyMembersRelationship> RelationshipsAsSecondMember { get; set; } = new HashSet<FamilyMembersRelationship>();
    }
}
