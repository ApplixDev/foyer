using Foyer.People;
using Shouldly;
using System;
using Xunit;

namespace Foyer.Tests.People
{
    public class PersonManager_Tests : FoyerTestBase
    {
        private readonly IPersonManager _personManager;

        public PersonManager_Tests()
        {
            _personManager = Resolve<IPersonManager>();
        }

        [Fact]
        public void Should_Return_True_If_Any_Person_With_Given_FullName_And_Birthdate_Exists()
        {
            var existingPerson = GetPersonById(1);

            WithUnitOfWork(() => _personManager.PersonExists(existingPerson).ShouldBeTrue());
        }

        [Fact]
        public void Should_Return_False_If_Person_Does_Not_Exists()
        {
            WithUnitOfWork(() => _personManager.PersonExists(new Person()).ShouldBeFalse());
        }

        [Fact]
        public void Should_Return_False_If_No_Person_With_Given_Personal_Informations_Exists()
        {
            var existingPerson = GetPersonById(1);

            var PersonFirstNameDoNotMatch = new Person
            {
                FirstName = "No",
                LastName = existingPerson.LastName,
                BirthDate = existingPerson.BirthDate
            };

            var PersonLastNameDoNotMatch = new Person
            {
                FirstName = existingPerson.FirstName,
                LastName = "No",
                BirthDate = existingPerson.BirthDate
            };

            var PersonBirthdateDoNotMatch = new Person
            {
                FirstName = existingPerson.FirstName,
                LastName = existingPerson.LastName,
                BirthDate = DateTime.Now
            };

            WithUnitOfWork(() =>
            {
                _personManager.PersonExists(PersonFirstNameDoNotMatch).ShouldBeFalse();
                _personManager.PersonExists(PersonLastNameDoNotMatch).ShouldBeFalse();
                _personManager.PersonExists(PersonBirthdateDoNotMatch).ShouldBeFalse();
            });
        }
    }
}
