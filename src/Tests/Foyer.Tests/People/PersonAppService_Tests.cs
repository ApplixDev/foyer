using Abp.Authorization.Users;
using Abp.Runtime.Validation;
using Abp.UI;
using Foyer.People;
using Foyer.People.Dto;
using Foyer.Tests.Utilities;
using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shouldly;
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

            _personAppService.Create(loCelso);

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
            Should.Throw<UserFriendlyException>(() => _personAppService.Create(salah))
                .Message.ShouldBe("This Person already exist");

            UsingDbContext(context => context.People.Count().ShouldBe(initialPeopleCount));
        }

        [Fact]
        public void Should_Not_Create_New_Person_If_FirstName_Is_Null()
        {
            var initialPeopleCount = UsingDbContext(context => context.People.Count());

            var personWithNullFirstName = new CreatePersonInput()
            {
                LastName = "Doe",
                Gender = Gender.Male,
                BirthDate = new DateTime(1992, 6, 15),
                BirthPlace = "USA",
                OtherDetails = "We don't know the first name of this person"
            };
            Should.Throw<AbpValidationException>(() => _personAppService.Create(personWithNullFirstName));

            UsingDbContext(context => context.People.Count().ShouldBe(initialPeopleCount));
        }

        [Fact]
        public void Should_Not_Create_New_Person_If_FirstName_Length_Is_Oversize()
        {
            var initialPeopleCount = UsingDbContext(context => context.People.Count());

            var OversizedFirstName = Strings.GenerateRandomString(AbpUserBase.MaxNameLength + 1);

            var personWithOversizeFirstNameLength = new CreatePersonInput()
            {
                FirstName = OversizedFirstName,
                LastName = "Doe",
                Gender = Gender.Male,
                BirthDate = new DateTime(1992, 6, 15),
                BirthPlace = "USA",
                OtherDetails = "We don't know the first name of this person"
            };
            Should.Throw<AbpValidationException>(() => _personAppService.Create(personWithOversizeFirstNameLength));

            UsingDbContext(context => context.People.Count().ShouldBe(initialPeopleCount));
        }

        [Fact]
        public void Should_Not_Create_New_Person_If_LastName_Is_Null()
        {
            var initialPeopleCount = UsingDbContext(context => context.People.Count());

            var personWithNullLastName = new CreatePersonInput()
            {
                FirstName = "John",
                Gender = Gender.Male,
                BirthDate = new DateTime(1992, 6, 15),
                BirthPlace = "USA",
                OtherDetails = "We don't know the last name of this person"
            };
            Should.Throw<AbpValidationException>(() => _personAppService.Create(personWithNullLastName));

            UsingDbContext(context => context.People.Count().ShouldBe(initialPeopleCount));
        }

        [Fact]
        public void Should_Not_Create_New_Person_If_LastName_Length_Is_Oversize()
        {
            var initialPeopleCount = UsingDbContext(context => context.People.Count());

            var OversizedLastName = Strings.GenerateRandomString(AbpUserBase.MaxNameLength + 1);

            var personWithOversizeLastNameLength = new CreatePersonInput()
            {
                FirstName = "John",
                LastName = OversizedLastName,
                Gender = Gender.Male,
                BirthDate = new DateTime(1992, 6, 15),
                BirthPlace = "USA",
                OtherDetails = "We don't know the last name of this person"
            };
            Should.Throw<AbpValidationException>(() => _personAppService.Create(personWithOversizeLastNameLength));

            UsingDbContext(context => context.People.Count().ShouldBe(initialPeopleCount));
        }

        [Fact]
        public void Should_Not_Create_New_Person_If_Gender_Is_Invalid()
        {
            var initialPeopleCount = UsingDbContext(context => context.People.Count());

            var personWithInvalidGenderValue = new CreatePersonInput()
            {
                FirstName = "John",
                LastName = "Doe",
                BirthDate = new DateTime(1992, 6, 15),
                BirthPlace = "USA",
                OtherDetails = "We don't know the gender of this person"
            };
            Should.Throw<AbpValidationException>(() => _personAppService.Create(personWithInvalidGenderValue));

            personWithInvalidGenderValue.Gender = (Gender)3; //Value can only be Female = 1 or Male = 2.
            Should.Throw<AbpValidationException>(() => _personAppService.Create(personWithInvalidGenderValue));

            UsingDbContext(context => context.People.Count().ShouldBe(initialPeopleCount));
        }

        [Fact]
        public void Should_Not_Create_Person_With_Oversized_Details_Length()
        {
            var initialPeopleCount = UsingDbContext(context => context.People.Count());

            var OversizedDetails = Strings.GenerateRandomString(Person.MaxDetailsLength + 1);

            var personWithOversizeDetailsLength = new CreatePersonInput()
            {
                FirstName = "John",
                LastName = "Doe",
                Gender = Gender.Male,
                BirthDate = new DateTime(1992, 6, 15),
                BirthPlace = "USA",
                OtherDetails = OversizedDetails
            };
            Should.Throw<AbpValidationException>(() => _personAppService.Create(personWithOversizeDetailsLength));

            UsingDbContext(context => context.People.Count().ShouldBe(initialPeopleCount));
        }

        [Fact]
        public void Should_Not_Create_Person_With_Oversized_BirthPlace_Length()
        {
            var initialPeopleCount = UsingDbContext(context => context.People.Count());

            var OversizedBirthPlace = Strings.GenerateRandomString(Person.MaxBirthPlaceNameLength + 1);

            var personWithOversizeBirthPlaceLength = new CreatePersonInput()
            {
                FirstName = "John",
                LastName = "Doe",
                Gender = Gender.Male,
                BirthDate = new DateTime(1992, 6, 15),
                BirthPlace = OversizedBirthPlace
            };
            Should.Throw<AbpValidationException>(() => _personAppService.Create(personWithOversizeBirthPlaceLength));

            UsingDbContext(context => context.People.Count().ShouldBe(initialPeopleCount));
        }
    }
}
