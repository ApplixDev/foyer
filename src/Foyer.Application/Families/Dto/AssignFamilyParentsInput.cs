using System.ComponentModel.DataAnnotations;

namespace Foyer.Families.Dto
{
    public class AssignFamilyParentsInput
    {
        [Range(1, int.MaxValue)]
        public int FamilyId { get; set; }

        [Range(1, int.MaxValue)]
        public int FatherId { get; set; }

        [Range(1, int.MaxValue)]
        public int MotherId { get; set; }
    }
}
