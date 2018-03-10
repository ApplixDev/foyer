using System.ComponentModel.DataAnnotations;

namespace Foyer.Families.Dto
{
    public class AssignFamilyParentInput
    {
        [Range(1, int.MaxValue)]
        public int ParentId { get; set; }

        [Range(1, int.MaxValue)]
        public int FamilyId { get; set; }
    }
}