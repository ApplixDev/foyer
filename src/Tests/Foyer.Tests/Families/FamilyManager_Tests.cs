using Foyer.Families;
using Foyer.People;
using Xunit;
using Shouldly;

namespace Foyer.Tests.Families
{
    public class FamilyManager_Tests : FoyerTestBase
    {
        private readonly IFamilyManager _familyManager;

        public FamilyManager_Tests()
        {
            _familyManager = Resolve<IFamilyManager>();
        }

        [Fact]
        public void Should_Assign_Person_Head_Of_Family()
        {
            var person = new Person() { Id = 1 };
            var family = new Family() { Id = 1 };

            _familyManager.AssignFamilyFather(family, person);

            family.HusbandId.ShouldBe(person.Id);
        }

        [Fact]
        public void Should_Do_Nothing_If_Assigned_Person_Is_Already_Head_Of_Family()
        {
            var person = new Person { Id = 1 };

            var family = new Family
            {
                Id = 1,
                HusbandId = person.Id
            };

            _familyManager.AssignFamilyFather(family, person);

            family.HusbandId.ShouldBe(person.Id);
        }
    }
}
