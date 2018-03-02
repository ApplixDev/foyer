using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Foyer.People.Dto
{
    [AutoMapFrom(typeof(Person))] //Map PersonDto from Person
    public class PersonDto : EntityDto
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public Gender Gender { get; set; }

        public DateTime? BirthDate { get; set; }

        public string BirthPlace { get; set; }

        public string OtherDetails { get; set; }

        public bool IsDeleted { get; set; }
    }
}
