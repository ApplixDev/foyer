﻿using Abp.Authorization.Users;
using Abp.AutoMapper;
using System;
using System.ComponentModel.DataAnnotations;

namespace Foyer.People.Dto
{
    [AutoMapTo(typeof(Person))]
    public class CreatePersonInput
    {
        [Required]
        [StringLength(AbpUserBase.MaxNameLength)]
        public string FirstName { get; set; }

        [Required]
        [StringLength(AbpUserBase.MaxNameLength)]
        public string LastName { get; set; }

        [Required]
        public Gender Gender { get; set; }

        public DateTime? BirthDate { get; set; }

        public DateTime? BirthPlace { get; set; }

        [StringLength(Person.MaxDetailsLength)]
        public string OtherDetails { get; set; }
    }
}
