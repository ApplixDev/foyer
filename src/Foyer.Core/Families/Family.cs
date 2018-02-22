using Abp.Domain.Entities;
using Foyer.People;
using Foyer.FamilyRelationships;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Foyer.Families
{
    public class Family : Entity, ISoftDelete
    {
        public const int MaxDetailsLength = 64 * 1024; //64KB
        public const int MaxFamilyNameLength = 32;

        /// <summary>
        /// Head of family.
        /// </summary>
        [ForeignKey(nameof(HeadOfFamilyId))]
        public virtual Person HeadOfFamily { get; set; }
        public virtual int? HeadOfFamilyId { get; set; }

        /// <summary>
        /// Family name.
        /// </summary>
        [StringLength(MaxFamilyNameLength)]
        public virtual string FamilyName { get; set; }

        /// <summary>
        /// Other details of the family.
        /// </summary>
        [StringLength(MaxDetailsLength)]
        public virtual string OtherDetails { get; set; }

        /// <summary>
        /// Widing date.
        /// </summary>
        public virtual DateTime? WidingDate { get; set; }

        /// <summary>
        /// Separation date.
        /// </summary>
        public virtual DateTime? DivorceDate { get; set; }

        /// <summary>
        /// Date of entry.
        /// </summary>
        public virtual DateTime? EntryDate { get; set; }

        /// <summary>
        /// Used to mark family as deleted.
        /// </summary>
        public virtual bool IsDeleted { get; set; }

        /// <summary>
        /// Family members relationships.
        /// </summary>
        public virtual ICollection<FamilyRelationship> FamilyRelationships { get; set; } = new HashSet<FamilyRelationship>();
    }
}
