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
        public void Should_Assign_Person_As_Head_Of_Family()
        {
            Family family = new Family() { Id = 1 };
            Person person = new Person() { Id = 1 };

            _familyManager.AssignPersonHeadOfFamily(family, person);

            family.HeadOfFamilyId.ShouldBe(person.Id);
        }
    }
}
