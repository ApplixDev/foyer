using System.ComponentModel.DataAnnotations;

namespace Foyer.Families.Dto
{
    public class UpdateFamilyDto : CreateFamilyDto
    {
        [Range(1, int.MaxValue)]
        public int FamilyId { get; set; }
    }
}