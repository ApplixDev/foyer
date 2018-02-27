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
    }
}
