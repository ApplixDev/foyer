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
        public int? FatherId { get; set; }

        [Range(1, int.MaxValue)]
        public int? MotherId { get; set; }

        public DateTime? WidingDate { get; set; }

        public DateTime? DivorceDate { get; set; }

        [StringLength(Family.MaxDetailsLength)]
        public string OtherDetails { get; set; }

        public bool IsDeleted { get; set; }

        public void AddValidationErrors(CustomValidationContext context)
        {
            if (!(FatherId.HasValue || MotherId.HasValue))
            {
                context.Results.Add(new ValidationResult("At least one of the parents must be set!"));
            }
            else if (FatherId == MotherId)
            {
                context.Results.Add(new ValidationResult("The same person can not be the father and mother at the same time"));
            }
        }
    }
}