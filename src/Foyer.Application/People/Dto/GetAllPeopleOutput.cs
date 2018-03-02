using System.Collections.Generic;

namespace Foyer.People.Dto
{
    public class GetAllPeopleOutput
    {
        public IEnumerable<PersonDto> People { get; set; }
    }
}