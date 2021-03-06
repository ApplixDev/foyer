﻿using Abp.Authorization.Users;
using Abp.Domain.Entities;
using Foyer.FamilyRelationships;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Foyer.People
{
    public class Person : Entity, ISoftDelete
    {
        public const int MaxDetailsLength = 64 * 1024; //64KB
        public const int MaxBirthPlaceNameLength = 328;

        /// <summary>
        /// First name.
        /// </summary>
        [Required]
        [StringLength(AbpUserBase.MaxNameLength)]
        public virtual string FirstName { get; set; }

        /// <summary>
        /// Last name.
        /// </summary>
        [Required]
        [StringLength(AbpUserBase.MaxNameLength)]
        public virtual string LastName { get; set; }

        /// <summary>
        /// Male or female.
        /// </summary>
        [EnumDataType(typeof(Gender))]
        public virtual Gender Gender { get; set; }

        /// <summary>
        /// Date of birth.
        /// </summary>
        public virtual DateTime BirthDate { get; set; }

        /// <summary>
        /// Place of birth.
        /// </summary>
        [StringLength(MaxBirthPlaceNameLength)]
        public virtual string BirthPlace { get; set; }

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
        /// Represents the family relationships that this person has with people.
        /// </summary>
        public virtual ICollection<FamilyRelationship> RelationshipsAsPrincipalPerson { get; set; } = new HashSet<FamilyRelationship>();

        /// <summary>
        /// Represents the family relationships where this person was related to people.
        /// </summary>
        public virtual ICollection<FamilyRelationship> RelationshipsAsRelatedPerson { get; set; } = new HashSet<FamilyRelationship>();
    }
}
