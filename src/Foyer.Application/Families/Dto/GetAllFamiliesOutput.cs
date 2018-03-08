using System.Collections.Generic;

namespace Foyer.Families.Dto
{
    public class GetAllFamiliesOutput
    {
        public IEnumerable<FamilyDto> Families { get; set; }
    }
}