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
        public void Should_Return_False_If_Person_Does_Not_Exists()
        {
            _personManager.PersonExists(new Person()).ShouldBeFalse();
        }

        [Fact]
        public void Should_Return_False_If_No_Person_With_Given_FullName_And_Birthdate_Exists()
        {
            var salah = GetPerson("Mohamed", "Salah");

            var PersonFirstNameNotMatch = new Person
            {
                FirstName = "No",
                LastName = salah.LastName,
                BirthDate = salah.BirthDate
            };

            var PersonLastNameNotMatch = new Person
            {
                FirstName = salah.FirstName,
                LastName = "No",
                BirthDate = salah.BirthDate
            };

            var PersonBirthdateNotMatch = new Person
            {
                FirstName = salah.FirstName,
                LastName = salah.LastName,
                BirthDate = DateTime.Now
            };

            _personManager.PersonExists(PersonFirstNameNotMatch).ShouldBeFalse();
            _personManager.PersonExists(PersonLastNameNotMatch).ShouldBeFalse();
            _personManager.PersonExists(PersonBirthdateNotMatch).ShouldBeFalse();
        }

        [Fact]
        public void Should_Return_True_If_Any_Person_With_Given_FullName_And_Birthdate_Exists()
        {
            var salah = GetPerson("Mohamed", "Salah");

            _personManager.PersonExists(salah).ShouldBeTrue();
        }
    }
}
