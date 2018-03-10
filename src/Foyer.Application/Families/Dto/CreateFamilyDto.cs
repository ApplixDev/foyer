using Abp.Authorization.Users;
using Abp.AutoMapper;
using Abp.Runtime.Validation;
using System;
using System.ComponentModel.DataAnnotations;

namespace Foyer.Families.Dto
{
    [AutoMapTo(typeof(Family))]
    public class CreateFamilyDto : ICustomValidate
    {
        [StringLength(AbpUserBase.MaxNameLength)]
        public string FamilyName { get; set; }

        [Range(1, int.MaxValue)]
        public int? HusbandId { get; set; }

        [Range(1, int.MaxValue)]
        public int? WifeId { get; set; }

        public DateTime? WidingDate { get; set; }

        public DateTime? DivorceDate { get; set; }

        [StringLength(Family.MaxDetailsLength)]
        public string OtherDetails { get; set; }

        public bool IsDeleted { get; set; }

        public void AddValidationErrors(CustomValidationContext context)
        {
            if (!(HusbandId.HasValue || WifeId.HasValue))
            {
                context.Results.Add(new ValidationResult("At least one of HusbandId or WifeId must be set!"));
            }
        }
    }
}