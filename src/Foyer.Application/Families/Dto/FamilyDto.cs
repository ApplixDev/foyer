using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using System;

namespace Foyer.Families.Dto
{
    [AutoMapFrom(typeof(Family))]
    public class FamilyDto : EntityDto
    {
        public string FamilyName { get; set; }

        public int? HeadOfFamilyId { get; set; }

        public DateTime? WidingDate { get; set; }

        public DateTime? DivorceDate { get; set; }

        public string OtherDetails { get; set; }

        public bool IsDeleted { get; set; }
    }
}