using Abp.Domain.Entities;
using Foyer.People;
using Foyer.FamilyRelationships;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Authorization.Users;

namespace Foyer.Families
{
    public class Family : Entity, ISoftDelete
    {
        public const int MaxDetailsLength = 64 * 1024; //64KB
        public const int MaxFamilyNameLength = 32;

        /// <summary>
        /// Husband.
        /// </summary>
        [ForeignKey(nameof(FatherId))]
        public virtual Person Father { get; set; }
        public virtual int? FatherId { get; set; }

        /// <summary>
        /// Wife.
        /// </summary>
        [ForeignKey(nameof(MotherId))]
        public virtual Person Mother { get; set; }
        public virtual int? MotherId { get; set; }

        /// <summary>
        /// Family name, can be used for family nickname or for shortened family name.
        /// </summary>
        [StringLength(AbpUserBase.MaxNameLength)]
        public virtual string FamilyName { get; set; }

        /// <summary>
        /// Widing date.
        /// </summary>
        public virtual DateTime? WidingDate { get; set; }

        /// <summary>
        /// Separation date.
        /// </summary>
        public virtual DateTime? DivorceDate { get; set; }

        /// <summary>
        /// Other details of the family.
        /// </summary>
        [StringLength(MaxDetailsLength)]
        public virtual string OtherDetails { get; set; }

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
