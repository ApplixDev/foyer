using Foyer.Families;
using Foyer.People;
using Xunit;
using Shouldly;
using Abp.UI;
using NSubstitute;
using Abp.Domain.Repositories;

namespace Foyer.Tests.Families
{
    public class FamilyManager_Tests : FoyerTestBase
    {
        private readonly IRepository<Family> _familyRepository;
        private readonly IFamilyManager _familyManager;

        public FamilyManager_Tests()
        {
            _familyRepository = Resolve<IRepository<Family>>();

            //Partial substitutes allow us to create an object that acts like a real instance of a class,
            //and selectively substitute for specific parts of that object.
            //This is useful for when we need a substitute to have real behaviour except for a single method
            //that we want to replace, or when we just want to spy on what calls are being made.
            _familyManager = Substitute.ForPartsOf<FamilyManager>(_familyRepository);
        }

        #region Assign family parents tests
        [Fact]
        public void Should_Assign_Person_As_Family_Father()
        {
            var family = new Family { Id = 1 };
            var men = new Person { Id = 1, Gender = Gender.Male };

            _familyManager.AssignFamilyFather(family, men);

            family.FatherId.ShouldBe(men.Id);
        }

        [Fact]
        public void Should_Not_Throw_Exception_If_Person_Is_Already_Assigned_Family_Father()
        {
            var men = new Person { Id = 1, Gender = Gender.Male };
            var family = new Family { Id = 1, FatherId = men.Id };

            Should.NotThrow(() => _familyManager.AssignFamilyFather(family, men));

            family.FatherId.ShouldBe(men.Id);
        }

        [Fact]
        public void Should_Throw_UserFriendlyException_If_Person_Assigned_Family_Father_Is_Not_A_Male()
        {
            var family = new Family { Id = 1 };
            var women = new Person { Id = 1, Gender = Gender.Female };

            Should.Throw<UserFriendlyException>(() => _familyManager.AssignFamilyFather(family, women))
                .Message.ShouldBe("The family father must be a male");
        }

        [Fact]
        public void Should_Assign_Person_As_Family_Mother()
        {
            var family = new Family { Id = 1 };
            var women = new Person { Id = 1, Gender = Gender.Female };

            _familyManager.AssignFamilyMother(family, women);

            family.MotherId.ShouldBe(women.Id);
        }

        [Fact]
        public void Should_Not_Throw_Exception_If_Person_Is_Already_Assigned_Family_Mother()
        {
            var women = new Person { Id = 1, Gender = Gender.Female };

            var family = new Family { Id = 1, MotherId = women.Id };

            Should.NotThrow(() => _familyManager.AssignFamilyMother(family, women));

            family.MotherId.ShouldBe(women.Id);
        }

        [Fact]
        public void Should_Throw_UserFriendlyException_If_Person_Assigned_Family_Mother_Is_Not_A_Female()
        {
            var family = new Family { Id = 1 };
            var men = new Person { Id = 1, Gender = Gender.Male };

            Should.Throw<UserFriendlyException>(() => _familyManager.AssignFamilyMother(family, men))
                .Message.ShouldBe("The family mother must be a female");
        }

        [Fact]
        public void Should_Call_AssignFamilyFather_And_AssignFamilyMother()
        {
            var family = new Family { Id = 1 };
            var father = new Person { Id = 1, Gender = Gender.Male };
            var mother = new Person { Id = 2, Gender = Gender.Female };

            _familyManager.AssignFamilyParents(family, father, mother);

            _familyManager.Received().AssignFamilyFather(family, father);
            _familyManager.Received().AssignFamilyMother(family, mother);
        }
        #endregion

        #region Family exists tests
        [Fact]
        public void Should_Return_True_If_Family_With_Defined_Parents_Ids_Exists()
        {
            var salahFamily = GetFamilyFromParentName("Mohamed", "Salah");

            UsingDbContext((context) => _familyManager.ParentsFamilyExists(salahFamily).ShouldBeTrue());
        }
        #endregion
    }
}
