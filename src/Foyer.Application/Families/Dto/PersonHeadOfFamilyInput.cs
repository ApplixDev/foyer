using System.ComponentModel.DataAnnotations;

namespace Foyer.Families.Dto
{
    public class PersonHeadOfFamilyInput
    {
        [Range(1, int.MaxValue)]
        public int PersonId { get; set; }

        [Range(1, int.MaxValue)]
        public int FamilyId { get; set; }
    }
}