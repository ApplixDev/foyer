using Abp.Authorization.Users;
using Abp.AutoMapper;
using System;
using System.ComponentModel.DataAnnotations;

namespace Foyer.Families.Dto
{
    [AutoMapTo(typeof(Family))]
    public class CreateFamilyInput
    {
        [Range(1, int.MaxValue)]
        public int HeadOfFamilyId { get; set; }

        [StringLength(AbpUserBase.MaxNameLength)]
        public string FamilyName { get; set; }

        [StringLength(Family.MaxDetailsLength)]
        public string OtherDetails { get; set; }

        public DateTime? WidingDate { get; set; }

        public DateTime? DivorceDate { get; set; }

        public bool IsDeleted { get; set; }
    }
}