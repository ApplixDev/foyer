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
using System.Data.Entity;
using Abp.Domain.Entities;

namespace Foyer.Tests.People
{
    public class PersonAppService_Tests : FoyerTestBase
    {
        private readonly IPersonAppService _personAppService;

        public PersonAppService_Tests()
        {
            _personAppService = Resolve<IPersonAppService>();
        }

        #region Create methode tests
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
            });
        }

        [Fact]
        public async Task Should_Create_New_Person_Async()
        {
            //Act
            await _personAppService.CreateAsync(
                new CreatePersonInput
                {
                    FirstName = "John",
                    LastName = "Nash",
                    Gender = Gender.Male,
                    BirthDate = new DateTime(1980, 2, 28),
                    BirthPlace = "USA",
                    OtherDetails = "Nothing"
                });

            await UsingDbContextAsync(async context =>
            {
                var johnNash = await context.People.FirstOrDefaultAsync(p => p.FirstName == "John" && p.LastName == "Nash" && p.BirthDate == new DateTime(1980, 2, 28));
                johnNash.ShouldNotBeNull();
            });
        }

        [Fact]
        public void Should_Not_Create_New_Person_If_Person_Exist()
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
        public void Should_Not_Create_New_Person_If_Gender_Value_Is_Invalid()
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
        public void Should_Not_Create_New_Person_If_Details_Length_Is_Oversize()
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
        public void Should_Not_Create_New_Person_If_BirthPlace_Length_Is_Oversize()
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
        #endregion

        #region Update methode tests
        [Fact]
        public void Should_Update_Person()
        {
            var salah = GetPerson("Mohamed", "Salah");
            var updatedSalah = new UpdatePersonInput
            {
                PersonId = salah.Id,
                FirstName = "Mohammed",
                LastName = "Salaheddine",
                BirthDate = new DateTime(1987, 1, 1)
            });

            _personAppService.Update(updatedSalah);

            UsingDbContext(context =>
            {
                var foundSalah = context.People.Single(p => p.Id == salah.Id);

                foundSalah.ShouldNotBeNull();
                foundSalah.FirstName.ShouldBe("Mohammed");
                foundSalah.LastName.ShouldBe("Salaheddine");
                foundSalah.BirthDate.ShouldBe(updatedSalah.BirthDate);
            });
        }

        [Fact]
        public void Should_Not_Update_Or_Add_Person_If_Person_Id_Does_Not_Exist()
        {
            //Arrange
            var initialPeopleCount = UsingDbContext(context => context.People.Count());
            var randomId = initialPeopleCount + 1;
            var foundPerson = GetPerson(randomId);

            //So the test works correctly.
            foundPerson.ShouldBeNull(@"We should not find a person with the random Id.
                If foundPerson is not null, try change the randomId value with not existing Id.");

            //All input fields are valid but this person do not exist.
            var notExistingPerson = new UpdatePersonInput()
            {
                PersonId = randomId,
                FirstName = "Lo",
                LastName = "Celso",
                Gender = Gender.Male,
                BirthDate = new DateTime(1996, 4, 9),
                OtherDetails = "This person does not exist in database seed"
            };
            Should.Throw<EntityNotFoundException>(() => _personAppService.Update(notExistingPerson));

            UsingDbContext(context => context.People.Count().ShouldBe(initialPeopleCount));
        }

        [Fact]
        public void Should_Not_Update_Person_If_Person_Id_Is_Out_Of_Range()
        {
            var initialPeopleCount = UsingDbContext(context => context.People.Count());

            var newPerson = new UpdatePersonInput()
            {
                //PersonID = 0 by default
                FirstName = "Lo",
                LastName = "Celso",
                Gender = Gender.Male,
                BirthDate = new DateTime(1996, 4, 9),
                OtherDetails = "This person does not exist in database seed"
            };
            Should.Throw<AbpValidationException>(() => _personAppService.Update(newPerson), "PersonId = 0 should be out of range");

            UsingDbContext(context => context.People.Count().ShouldBe(initialPeopleCount));
        }

        [Fact]
        public void Should_Not_Update_Person_If_FirstName_Is_Null()
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
        public void Should_Not_Update_Person_If_FirstName_Length_Is_Oversize()
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
        public void Should_Not_Update_Person_If_LastName_Is_Null()
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
        public void Should_Not_Update_Person_If_LastName_Length_Is_Oversize()
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
        public void Should_Not_Update_Person_If_Gender_Value_Is_Invalid()
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
        public void Should_Not_Update_Person_If_Details_Length_Is_Oversize()
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
        public void Should_Not_Update_Person_If_BirthPlace_Length_Is_Oversize()
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

        private Person GetPerson(int personId)
        {
            return UsingDbContext(context => context.People.FirstOrDefault(p => p.Id == personId));
        }

        private Person GetPerson(string firstName, string lastName)
        {
            return UsingDbContext(context => context.People.Single(p => p.FirstName == firstName && p.LastName == lastName));
        }
        #endregion

        #region Get all people test
        [Fact]
        public async Task GetPeople_Test()
        {
            //Act
            var output = await _personAppService.GetAllPeople();

            //Assert
            output.People.Count().ShouldBeGreaterThan(0);
        }
        #endregion
    }
}
