using Abp.AutoMapper;
using System;

namespace Foyer.People.Dto
{
    public class CreatePersonInput
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public Gender Gender { get; set; }

        public DateTime? DateOfBirth { get; set; }

        public DateTime? PlaceOfBirth { get; set; }

        public string OtherDetails { get; set; }
    }
}
