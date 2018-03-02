using System.ComponentModel.DataAnnotations;

namespace Foyer.People.Dto
{
    public class GetPersonInput
    {
        [Range(1, int.MaxValue)]
        public int PersonId { get; set; }
    }
}