using System;
using System.ComponentModel.DataAnnotations;

namespace Foyer.People.Dto
{
    public class UpdatePersonInput
    {
        [Range(1, int.MaxValue)]
        public int PersonId { get; set; }

        [StringLength(Person.MaxNameLength)]
        public string FirstName { get; set; }

        [StringLength(Person.MaxNameLength)]
        public string LastName { get; set; }

        public Gender? Gender { get; set; }

        public DateTime? DateOfBirth { get; set; }

        public DateTime? PlaceOfBirth { get; set; }

        [StringLength(Person.MaxDetailsLength)]
        public string OtherDetails { get; set; }
    }
}