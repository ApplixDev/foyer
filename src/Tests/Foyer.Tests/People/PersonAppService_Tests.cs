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

        #region Create person tests
        [Fact]
        public void Should_Create_New_Person()
        {
            var initialPeopleCount = UsingDbContext(context => context.People.Count());

            var loCelso = new CreatePersonDto
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

                var foundLoCelso = context.People.FirstOrDefault
                (
                    p => p.FirstName == loCelso.FirstName
                    && p.LastName == loCelso.LastName
                    && p.Gender == loCelso.Gender
                    && p.BirthDate == loCelso.BirthDate
                );

                foundLoCelso.ShouldNotBeNull();
            });
        }

        [Fact]
        public async Task Should_Create_New_Person_Async()
        {
            var johnNash = new CreatePersonDto
            {
                FirstName = "John",
                LastName = "Nash",
                Gender = Gender.Male,
                BirthDate = new DateTime(1980, 2, 28),
                BirthPlace = "USA",
                OtherDetails = "Nothing"
            };

            await _personAppService.CreateAsync(johnNash);

            await UsingDbContextAsync(async context =>
            {
                var foundJohnNash = await context.People.FirstOrDefaultAsync
                (
                    p => p.FirstName == johnNash.FirstName
                    && p.LastName == johnNash.LastName
                    && p.Gender == johnNash.Gender
                    && p.BirthDate == johnNash.BirthDate
                );

                foundJohnNash.ShouldNotBeNull();
            });
        }

        [Fact]
        public void Should_Not_Create_New_Person_If_Person_Exist()
        {
            var initialPeopleCount = UsingDbContext(context => context.People.Count());

            var salah = new CreatePersonDto
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

            var personWithNullFirstName = new CreatePersonDto
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

            var oversizedFirstName = Strings.GenerateRandomString(AbpUserBase.MaxNameLength + 1);

            var personWithOversizedFirstNameLength = new CreatePersonDto
            {
                FirstName = oversizedFirstName,
                LastName = "Doe",
                Gender = Gender.Male,
                BirthDate = new DateTime(1992, 6, 15),
                BirthPlace = "USA",
                OtherDetails = "We don't know the first name of this person"
            };
            Should.Throw<AbpValidationException>(() => _personAppService.Create(personWithOversizedFirstNameLength));

            UsingDbContext(context => context.People.Count().ShouldBe(initialPeopleCount));
        }

        [Fact]
        public void Should_Not_Create_New_Person_If_LastName_Is_Null()
        {
            var initialPeopleCount = UsingDbContext(context => context.People.Count());

            var personWithNullLastName = new CreatePersonDto
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

            var oversizedLastName = Strings.GenerateRandomString(AbpUserBase.MaxNameLength + 1);

            var personWithOversizedLastNameLength = new CreatePersonDto
            {
                FirstName = "John",
                LastName = oversizedLastName,
                Gender = Gender.Male,
                BirthDate = new DateTime(1992, 6, 15),
                BirthPlace = "USA",
                OtherDetails = "We don't know the last name of this person"
            };
            Should.Throw<AbpValidationException>(() => _personAppService.Create(personWithOversizedLastNameLength));

            UsingDbContext(context => context.People.Count().ShouldBe(initialPeopleCount));
        }

        [Fact]
        public void Should_Not_Create_New_Person_If_Gender_Value_Is_Invalid()
        {
            var initialPeopleCount = UsingDbContext(context => context.People.Count());

            var personWithInvalidGenderValue = new CreatePersonDto
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

            var oversizedDetails = Strings.GenerateRandomString(Person.MaxDetailsLength + 1);

            var personWithOversizedDetailsLength = new CreatePersonDto
            {
                FirstName = "John",
                LastName = "Doe",
                Gender = Gender.Male,
                BirthDate = new DateTime(1992, 6, 15),
                BirthPlace = "USA",
                OtherDetails = oversizedDetails
            };
            Should.Throw<AbpValidationException>(() => _personAppService.Create(personWithOversizedDetailsLength));

            UsingDbContext(context => context.People.Count().ShouldBe(initialPeopleCount));
        }

        [Fact]
        public void Should_Not_Create_New_Person_If_BirthPlace_Length_Is_Oversize()
        {
            var initialPeopleCount = UsingDbContext(context => context.People.Count());

            var oversizedBirthPlace = Strings.GenerateRandomString(Person.MaxBirthPlaceNameLength + 1);

            var personWithOversizedBirthPlaceLength = new CreatePersonDto
            {
                FirstName = "John",
                LastName = "Doe",
                Gender = Gender.Male,
                BirthDate = new DateTime(1992, 6, 15),
                BirthPlace = oversizedBirthPlace
            };
            Should.Throw<AbpValidationException>(() => _personAppService.Create(personWithOversizedBirthPlaceLength));

            UsingDbContext(context => context.People.Count().ShouldBe(initialPeopleCount));
        }
        #endregion

        #region Update person tests
        [Fact]
        public void Should_Update_Person()
        {
            var existingPersonId = 1;

            var updatePerson = new UpdatePersonDto
            {
                PersonId = existingPersonId,
                FirstName = "NewName",
                LastName = "NewName",
                Gender = Gender.Female,
                BirthDate = new DateTime(2000, 1, 1)
            };

            _personAppService.Update(updatePerson);

            UsingDbContext(context =>
            {
                var existingPerson = context.People.Single(p => p.Id == existingPersonId);

                existingPerson.FirstName.ShouldBe(updatePerson.FirstName);
                existingPerson.LastName.ShouldBe(updatePerson.LastName);
                existingPerson.BirthDate.ShouldBe(updatePerson.BirthDate);
                existingPerson.Gender.ShouldBe(updatePerson.Gender);

                existingPerson.IsDeleted.ShouldBe(updatePerson.IsDeleted);
            });
        }

        [Fact]
        public void Should_Not_Update_Or_Add_Person_If_Person_Id_Does_Not_Exist()
        {
            //Arrange
            var initialPeopleCount = UsingDbContext(context => context.People.Count());
            var notExistingPersonId = GenerateNotExistingPersonId();

            //All input fields are valid but this person do not exist.
            var notExistingPerson = new UpdatePersonDto
            {
                PersonId = notExistingPersonId,
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
            var newPerson = new UpdatePersonDto
            {
                //PersonId = 0 by default
                FirstName = "Lo",
                LastName = "Celso",
                Gender = Gender.Male,
                BirthDate = new DateTime(1996, 4, 9),
                OtherDetails = "This person does not exist in database seed"
            };

            newPerson.PersonId.ShouldNotBeInRange(1, int.MaxValue);
            Should.Throw<AbpValidationException>(() => _personAppService.Update(newPerson), "PersonId = 0 should be out of range");
        }

        [Fact]
        public void Should_Not_Update_Person_If_FirstName_Is_Null()
        {
            var existingPerson = GetPersonById(1);
            var personWithNullFirstName = new UpdatePersonDto
            {
                PersonId = existingPerson.Id,
                LastName = existingPerson.LastName,
                Gender = existingPerson.Gender,
                BirthDate = existingPerson.BirthDate,
                BirthPlace = existingPerson.BirthPlace,
                OtherDetails = existingPerson.OtherDetails
            };

            personWithNullFirstName.FirstName.ShouldBeNull();
            Should.Throw<AbpValidationException>(() => _personAppService.Update(personWithNullFirstName));
        }

        [Fact]
        public void Should_Not_Update_Person_If_FirstName_Length_Is_Oversize()
        {
            var existingPerson = GetPersonById(1);
            var oversizedFirstName = Strings.GenerateRandomString(AbpUserBase.MaxNameLength + 1);

            var salahWithOversizedFirstNameLength = new UpdatePersonDto
            {
                PersonId = existingPerson.Id,
                FirstName = oversizedFirstName,
                LastName = existingPerson.LastName,
                Gender = existingPerson.Gender,
                BirthDate = existingPerson.BirthDate,
                BirthPlace = existingPerson.BirthPlace,
                OtherDetails = existingPerson.OtherDetails
            };

            Should.Throw<AbpValidationException>(() => _personAppService.Update(salahWithOversizedFirstNameLength));
        }

        [Fact]
        public void Should_Not_Update_Person_If_LastName_Is_Null()
        {
            var existingPerson = GetPersonById(1);

            var personWithNullLastName = new UpdatePersonDto
            {
                PersonId = existingPerson.Id,
                FirstName = existingPerson.FirstName,
                Gender = existingPerson.Gender,
                BirthDate = existingPerson.BirthDate,
                BirthPlace = existingPerson.BirthPlace,
                OtherDetails = existingPerson.OtherDetails
            };

            personWithNullLastName.LastName.ShouldBeNull();
            Should.Throw<AbpValidationException>(() => _personAppService.Update(personWithNullLastName));
        }

        [Fact]
        public void Should_Not_Update_Person_If_LastName_Length_Is_Oversize()
        {
            var existingPerson = GetPersonById(1);
            var oversizedLastName = Strings.GenerateRandomString(AbpUserBase.MaxNameLength + 1);

            var personWithOversizedLastNameLength = new UpdatePersonDto
            {
                PersonId = existingPerson.Id,
                FirstName = existingPerson.FirstName,
                LastName = oversizedLastName,
                Gender = existingPerson.Gender,
                BirthDate = existingPerson.BirthDate,
                BirthPlace = existingPerson.BirthPlace,
                OtherDetails = existingPerson.OtherDetails
            };

            Should.Throw<AbpValidationException>(() => _personAppService.Update(personWithOversizedLastNameLength));
        }

        [Fact]
        public void Should_Not_Update_Person_If_Gender_Value_Is_Invalid()
        {
            var existingPerson = GetPersonById(1);

            var personWithInvalidGenderValue = new UpdatePersonDto
            {
                PersonId = existingPerson.Id,
                FirstName = existingPerson.FirstName,
                LastName = existingPerson.LastName,
                BirthDate = existingPerson.BirthDate,
                BirthPlace = existingPerson.BirthPlace,
                OtherDetails = existingPerson.OtherDetails
            };
            Should.Throw<AbpValidationException>(() => _personAppService.Update(personWithInvalidGenderValue));

            personWithInvalidGenderValue.Gender = (Gender)3; //Value can only be Female = 1 or Male = 2.
            Should.Throw<AbpValidationException>(() => _personAppService.Update(personWithInvalidGenderValue));
        }

        [Fact]
        public void Should_Not_Update_Person_If_BirthPlace_Length_Is_Oversize()
        {
            var existingPerson = GetPersonById(1);
            var oversizedBirthPlace = Strings.GenerateRandomString(Person.MaxBirthPlaceNameLength + 1);

            var personWithOversizedBirthPlaceLength = new UpdatePersonDto
            {
                PersonId = existingPerson.Id,
                FirstName = existingPerson.FirstName,
                LastName = existingPerson.LastName,
                Gender = existingPerson.Gender,
                BirthDate = existingPerson.BirthDate,
                BirthPlace = oversizedBirthPlace,
                OtherDetails = existingPerson.OtherDetails
            };

            Should.Throw<AbpValidationException>(() => _personAppService.Update(personWithOversizedBirthPlaceLength));
        }

        [Fact]
        public void Should_Not_Update_Person_If_Details_Length_Is_Oversize()
        {
            var existingPerson = GetPersonById(1);
            var oversizedDetails = Strings.GenerateRandomString(Person.MaxDetailsLength + 1);

            var personWithOversizedDetailsLength = new UpdatePersonDto
            {
                PersonId = existingPerson.Id,
                FirstName = existingPerson.FirstName,
                LastName = existingPerson.LastName,
                Gender = existingPerson.Gender,
                BirthDate = existingPerson.BirthDate,
                BirthPlace = existingPerson.BirthPlace,
                OtherDetails = oversizedDetails
            };

            Should.Throw<AbpValidationException>(() => _personAppService.Update(personWithOversizedDetailsLength));
        }
        #endregion

        #region Delete person tests
        [Fact]
        public void Should_Soft_Delete_Person()
        {
            var existingPersonId = 1;
            var initialPeopleCount = UsingDbContext(context => context.People.Count());

            _personAppService.Delete(new DeletePersonInput { PersonId = existingPersonId });

            UsingDbContext(context =>
            {
                //Filters are disabled inside UsingDbContext methods
                context.People.Count().ShouldBe(initialPeopleCount, "Soft delete is enabled, person should not be really deleted");
                context.People.First(p => p.Id == existingPersonId).IsDeleted.ShouldBeTrue();
            });
        }

        [Fact]
        public void Should_Not_Delete_Person_If_Person_Id_Does_Not_Exist()
        {
            var initialPeopleCount = UsingDbContext(context => context.People.Count());
            var notExistingPersonId = GenerateNotExistingPersonId();

            Should.Throw<UserFriendlyException>(() =>
            {
                _personAppService.Delete(new DeletePersonInput { PersonId = notExistingPersonId });
            }).Message.ShouldBe("This Person does not exist");

            UsingDbContext(context => context.People.Count().ShouldBe(initialPeopleCount));
        }

        [Fact]
        public void Should_Not_Delete_Person_If_Person_Is_Already_Soft_Deleted()
        {
            var initialPeopleCount = UsingDbContext(context => context.People.Count());
            var softDeletedPerson = UsingDbContext(context => context.People.First(p => p.IsDeleted));

            Should.Throw<UserFriendlyException>(() =>
            {
                _personAppService.Delete(new DeletePersonInput { PersonId = softDeletedPerson.Id });
            }).Message.ShouldBe("This Person does not exist");

            UsingDbContext(context => context.People.Count().ShouldBe(initialPeopleCount));
        }

        [Fact]
        public void Should_Not_Delete_Person_If_Person_Id_Is_Out_Of_Range()
        {
            Should.Throw<AbpValidationException>(() => 
            {
                _personAppService.Delete(new DeletePersonInput { PersonId = 0 });
            }, "PersonId = 0 should be out of range");
        }
        #endregion

        #region Get person tests
        [Fact]
        public void Should_Return_Found_PersonDto()
        {
            var existingPersonId = 1;

            var output = Should.NotThrow(() =>
            {
                return _personAppService.Get(new GetPersonInput { PersonId = existingPersonId });
            });

            output.ShouldBeOfType<PersonDto>();
            output.Id.ShouldBe(existingPersonId);
        }

        [Fact]
        public void Should_Throw_EntityNotFoundException_If_GetPersonInput_Id_Does_Not_Exist()
        {
            //Generate method make some assertions to test if generated Id does not exist.
            var notExistingPersonId = GenerateNotExistingPersonId();

            Should.Throw<EntityNotFoundException>(() => _personAppService.Get(new GetPersonInput { PersonId = notExistingPersonId }));
        }

        [Fact]
        public void Should_Throw_AbpValidationException_If_GetPersonInput_Id_Is_Out_Of_Range()
        {
            Should.Throw<AbpValidationException>(() =>
            {
                _personAppService.Get(new GetPersonInput { PersonId = 0 });
            }, "PersonId = 0 should be out of range");
        }
        #endregion

        #region Get all people tests
        [Fact]
        public async Task GetPeople_Test()
        {
            var output = await _personAppService.GetAllPeople();

            output.People.Count().ShouldBeGreaterThan(0);
        }
        #endregion
    }
}
