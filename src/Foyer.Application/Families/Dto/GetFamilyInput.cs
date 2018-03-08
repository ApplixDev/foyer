using System.ComponentModel.DataAnnotations;

namespace Foyer.Families.Dto
{
    public class GetFamilyInput
    {
        [Range(1, int.MaxValue)]
        public int FamilyId { get; set; }
    }
}