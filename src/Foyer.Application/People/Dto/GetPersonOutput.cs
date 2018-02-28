using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using System;

namespace Foyer.People.Dto
{
    public class GetPersonOutput
    {
        public int Id { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public Gender Gender { get; set; }

        public DateTime? BirthDate { get; set; }

        public string BirthPlace { get; set; }

        public string OtherDetails { get; set; }
    }
}