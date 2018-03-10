using Abp.Application.Services.Dto;
using Abp.Authorization.Users;
using Abp.AutoMapper;
using System;
using System.ComponentModel.DataAnnotations;

namespace Foyer.People.Dto
{
    [AutoMapTo(typeof(Person))]
    public class UpdatePersonDto : CreatePersonDto
    {
        [Range(1, int.MaxValue)]
        public int PersonId { get; set; }
    }
}