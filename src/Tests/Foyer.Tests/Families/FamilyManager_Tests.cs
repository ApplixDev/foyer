using Foyer.Families;
using Foyer.People;
using Xunit;
using Shouldly;
using Abp.UI;
using NSubstitute;
using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using Abp.Dependency;
using System;
using Abp.Localization;

namespace Foyer.Tests.Families
{
    public class FamilyManager_Tests : FoyerTestBase
    {
        private readonly IFamilyManager _familyManager;

        public FamilyManager_Tests()
        {
            _familyManager = Resolve<IFamilyManager>(Substitute.For<IFamilyManager>());
        }

        #region Assign family parents tests

        [Fact]
        public void Should_Throw_UserFriendlyException_If_Person_Assigned_Family_Father_Is_Not_A_Male()
        {
            var family = new Family { Id = 1 };
            var women = new Person { Id = 1, Gender = Gender.Female };

            Should.Throw<UserFriendlyException>(() => _familyManager.AssignFamilyFather(family, women))
                .Message.ShouldBe("The family father must be a male");
        }

        [Fact]
        public void Should_Throw_ApplicationException_If_Assigned_Father_Is_Transient()
        {
            var family = new Family { Id = 1 };
            var transientFather = new Person { FirstName = "Lo", LastName = "Celso", Gender = Gender.Male };

            Should.Throw<ApplicationException>(() => _familyManager.AssignFamilyFather(family, transientFather))
                .Message.ShouldBe("Assign transient person as family parent is not allowed, person id is required");
        }

        [Fact]
        public void Should_Assign_Existing_Person_As_Family_Father()
        {
            var family = new Family { Id = 1 };
            var man = new Person { Id = 1, Gender = Gender.Male };

            _familyManager.AssignFamilyFather(family, man);

            family.FatherId.ShouldBe(man.Id);
        }

        [Fact]
        public void Should_Not_Throw_Exception_If_Person_Is_Already_Assigned_Family_Father()
        {
            var father = new Person { Id = 1, Gender = Gender.Male };
            var familyWithFather = new Family { Id = 1, FatherId = father.Id };

            Should.NotThrow(() => _familyManager.AssignFamilyFather(familyWithFather, father));

            familyWithFather.FatherId.ShouldBe(father.Id);
        }

        [Fact]
        public void Should_Throw_UserFriendlyException_If_Person_Assigned_Family_Mother_Is_Not_A_Female()
        {
            var family = new Family { Id = 1 };
            var man = new Person { Id = 1, Gender = Gender.Male };

            Should.Throw<UserFriendlyException>(() => _familyManager.AssignFamilyMother(family, man))
                .Message.ShouldBe("The family mother must be a female");
        }

        [Fact]
        public void Should_Throw_ApplicationException_If_Assigned_Mother_Is_Transient()
        {
            var family = new Family { Id = 1 };
            var transientMother = new Person { FirstName = "Li", LastName = "Salsa", Gender = Gender.Female };

            Should.Throw<ApplicationException>(() => _familyManager.AssignFamilyMother(family, transientMother))
                .Message.ShouldBe("Assign transient person as family parent is not allowed, person id is required");
        }

        [Fact]
        public void Should_Assign_Existing_Person_As_Family_Mother()
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
        public void Should_Call_AssignFamilyFather_And_AssignFamilyMother()
        {
            var familyRepository = Substitute.For<IRepository<Family>>();

            //Partial substitutes allow us to create an object that acts like a real instance of a class,
            //and selectively substitute for specific parts of that object.
            //This is useful for when we need a substitute to have real behaviour except for a single method
            //that we want to replace, or when we just want to spy on what calls are being made.
            var familyManager = Substitute.ForPartsOf<FamilyManager>(familyRepository);

            var family = new Family { Id = 1 };
            var father = new Person { Id = 1, Gender = Gender.Male };
            var mother = new Person { Id = 2, Gender = Gender.Female };

            familyManager.AssignFamilyParents(family, father, mother);

            familyManager.Received().AssignFamilyFather(family, father);
            familyManager.Received().AssignFamilyMother(family, mother);
        }
        #endregion

        #region Family exists tests
        [Fact]
        public void Should_Return_True_If_Family_With_Defined_Parents_Ids_Exists()
        {
            var family = GetFamilyFromParentId(1);
            WithUnitOfWork(() => _familyManager.ParentsFamilyExists(family).ShouldBeTrue());
        }

        [Fact]
        public void Should_Return_False_If_Family_With_Defined_Parents_Ids_Do_Not_Exists()
        {
            var family = new Family
            {
                FatherId = GenerateNotExistingPersonId(),
                MotherId = GenerateNotExistingPersonId()
            };

            WithUnitOfWork(() => _familyManager.ParentsFamilyExists(family).ShouldBeFalse());
        }

        [Fact]
        public void Should_Not_Throw_Exception_If_We_Look_For_Single_Father_Family()
        {
            var family = new Family
            {
                FatherId = GenerateNotExistingPersonId(),
                MotherId = null
            };

            Should.NotThrow(() =>
            {
                WithUnitOfWork(() => _familyManager.ParentsFamilyExists(family).ShouldBeFalse());
            });
        }

        [Fact]
        public void Should_Not_Throw_Exception_If_We_Look_For_Single_Mother_Family()
        {
            var family = new Family
            {
                FatherId = null,
                MotherId = GenerateNotExistingPersonId()
            };

            Should.NotThrow(() =>
            {
                WithUnitOfWork(() => _familyManager.ParentsFamilyExists(family).ShouldBeFalse());
            });
        }
        #endregion
    }
}
