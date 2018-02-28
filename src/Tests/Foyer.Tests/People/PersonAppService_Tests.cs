using Abp.Runtime.Validation;
using Abp.UI;
using Foyer.People;
using Foyer.People.Dto;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Foyer.Tests.People
{
    public class PersonAppService_Tests : FoyerTestBase
    {
        private readonly IPersonAppService _personAppService;

        public PersonAppService_Tests()
        {
            _personAppService = Resolve<IPersonAppService>();
        }

        [Fact]
        public void Should_Create_New_Person()
        {
            var initialPeopleCount = UsingDbContext(context => context.People.Count());

            var loCelso = new CreatePersonInput()
            {
                FirstName = "Lo",
                LastName = "Celso",
                Gender = Gender.Male,
                BirthDate = new DateTime(1996, 4, 9)
            };

            _personAppService.CreatePerson(loCelso);

            UsingDbContext(context =>
            {
                context.People.Count().ShouldBe(initialPeopleCount + 1);

                var foundLoCelso = context.People.FirstOrDefault(
                    p => p.FirstName == loCelso.FirstName &&
                    p.LastName == loCelso.LastName &&
                    p.BirthDate == loCelso.BirthDate
                );

                foundLoCelso.ShouldNotBeNull();
                foundLoCelso.IsDeleted.ShouldBe(false);
            });
        }

        [Fact]
        public void Should_Not_Create_Existing_Person()
        {
            var initialPeopleCount = UsingDbContext(context => context.People.Count());

            var salah = new CreatePersonInput()
            {
                FirstName = "Mohamed",
                LastName = "Salah",
                Gender = Gender.Male,
                BirthDate = new DateTime(1992, 6, 15)
            };

            Should.Throw<UserFriendlyException>(() => _personAppService.CreatePerson(salah))
                .Message.ShouldBe("This Person already exist");

            UsingDbContext(context => context.People.Count().ShouldBe(initialPeopleCount));
        }

        [Fact]
        public void Should_Not_Create_New_Person_If_Not_Full_Informations_Are_Provided()
        {
            var initialPeopleCount = UsingDbContext(context => context.People.Count());

            var personWithoutFirstName = new CreatePersonInput()
            {
                LastName = "Doe",
                Gender = Gender.Male,
                BirthDate = new DateTime(1992, 6, 15),
                BirthPlace = "USA",
                OtherDetails = "We don't know the first name of this person"
            };
            Should.Throw<AbpValidationException>(() => _personAppService.CreatePerson(personWithoutFirstName));

            var personWithoutLastName = new CreatePersonInput()
            {
                FirstName = "John",
                Gender = Gender.Male,
                BirthDate = new DateTime(1992, 6, 15),
                BirthPlace = "USA",
                OtherDetails = "We don't know the last name of this person"
            };
            Should.Throw<AbpValidationException>(() => _personAppService.CreatePerson(personWithoutLastName));

            var personWithoutGender = new CreatePersonInput()
            {
                FirstName = "John",
                LastName = "Doe",
                BirthDate = new DateTime(1992, 6, 15),
                BirthPlace = "USA",
                OtherDetails = "We don't know the gender of this person"
            };
            personWithoutGender.Gender.ShouldBe(default(Gender));// Default value should be 0.
            Should.Throw<AbpValidationException>(() => _personAppService.CreatePerson(personWithoutGender));

            UsingDbContext(context => context.People.Count().ShouldBe(initialPeopleCount));
        }

        [Fact]
        public void Should_Not_Create_Person_With_Oversized_StringLength()
        {
            var initialPeopleCount = UsingDbContext(context => context.People.Count());

            //var unknownPerson = new CreatePersonInput()
            //{
            //    BirthDate = new DateTime(1992, 6, 15),
            //    OtherDetails = "Unknown person: we don't know the name of this person and his gender"
            //};

            //Should.Throw<UserFriendlyException>(() => _personAppService.CreatePerson(unknownPerson));

            UsingDbContext(context => context.People.Count().ShouldBe(initialPeopleCount));
        }
    }
}
