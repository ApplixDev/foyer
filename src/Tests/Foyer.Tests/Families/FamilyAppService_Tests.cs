using Abp.Domain.Entities;
using Abp.Runtime.Validation;
using Foyer.Families;
using Foyer.Families.Dto;
using Foyer.People;
using Shouldly;
using System.Linq;
using Xunit;

namespace Foyer.Tests.Families
{
    public class FamilyAppService_Tests : FoyerTestBase
    {
        private readonly IFamilyAppService _familyAppService;

        #region Ctor
        public FamilyAppService_Tests()
        {
            _familyAppService = Resolve<IFamilyAppService>();
        }
        #endregion

        #region Create

        [Fact]
        public void Should_Create_New_Family()
        {
            var newHusband = UsingDbContext(Context => Context.People.Add(new Person
            {
                FirstName = "Lo",
                LastName = "Celso",
                Gender = Gender.Male
            }));

            var newFamily = UsingDbContext(Context => Context.Families.Add(new Family()));

            _familyAppService.AssignFamilyFather(new AssignFamilyParentInput
            {
                ParentId = newHusband.Id,
                FamilyId = newFamily.Id
            });

            UsingDbContext(context =>
            {
                var createdPerson = context.People.FirstOrDefault(p => p.Id == newHusband.Id);
                createdPerson.ShouldNotBeNull();
                createdPerson.FirstName.ShouldBe(newHusband.FirstName);

                var createdFamily = context.Families.FirstOrDefault(f => f.Id == newFamily.Id);
                createdFamily.ShouldNotBeNull();
                createdFamily.FatherId.ShouldBe(createdPerson.Id);
            });
        }

        #endregion

        #region AssignPersonHeadOfFamily tests
        [Fact]
        public void Should_Assign_Person_As_Family_Father()
        {
            var newPerson = UsingDbContext(Context => Context.People.Add(new Person
            {
                FirstName = "Lo",
                LastName = "Celso",
                Gender = Gender.Male
            }));

            var newFamily = UsingDbContext(Context => Context.Families.Add(new Family()));
            
            _familyAppService.AssignFamilyFather(new AssignFamilyParentInput
            {
                ParentId = newPerson.Id,
                FamilyId = newFamily.Id
            });

            UsingDbContext(context => 
            {
                var createdPerson = context.People.FirstOrDefault(p => p.Id == newPerson.Id);
                createdPerson.ShouldNotBeNull();
                createdPerson.FirstName.ShouldBe(newPerson.FirstName);

                var createdFamily = context.Families.FirstOrDefault(f => f.Id == newFamily.Id);
                createdFamily.ShouldNotBeNull();
                createdFamily.FatherId.ShouldBe(createdPerson.Id);
            });
        }

        [Fact]
        public void Should_Not_Throw_Exception_If_Assigned_Person_Is_Already_Family_Father()
        {
            var salahId = GetPerson("Mohamed", "Salah").Id;
            var salahFamilyId = GetFamilyFromParentId(salahId).Id;

            Should.NotThrow(() => _familyAppService.AssignFamilyFather(new AssignFamilyParentInput
            {
                ParentId = salahId,
                FamilyId = salahFamilyId
            }));

            UsingDbContext(context =>
            {
                context.Families.FirstOrDefault(f => f.Id == salahFamilyId && f.FatherId == salahId).ShouldNotBeNull();
            });
        }

        [Fact]
        public void Should_Not_Assign_Not_Existing_Person_As_Family_Father()
        {
            var notExistingPersonId = GenerateNotExistingPersonId();

            var newFamily = UsingDbContext(Context => Context.Families.Add(new Family()));

            Should.Throw<EntityNotFoundException>(() =>
            {
                _familyAppService.AssignFamilyFather(new AssignFamilyParentInput
                {
                    ParentId = notExistingPersonId,
                    FamilyId = newFamily.Id
                });
            });
        }

        [Fact]
        public void Should_Not_Assign_Person_As_Family_Father_If_Family_Does_Not_Exist()
        {
            var notExistingFamilyId = GenerateNotExistingFamilyId();

            var salahId = GetPerson("Mohamed", "Salah").Id;

            Should.Throw<EntityNotFoundException>(() =>
            {
                _familyAppService.AssignFamilyFather(new AssignFamilyParentInput
                {
                    ParentId = salahId,
                    FamilyId = notExistingFamilyId
                });
            });
        }

        [Fact]
        public void Should_Throw_AbpValidationException_If_Father_Id_Is_Out_Of_Range()
        {
            var outOfRangePersonId = 0;

            var salahFamilyId = GetFamilyFromParentName("Mohamed", "Salah").Id;

            Should.Throw<AbpValidationException>(() =>
            {
                _familyAppService.AssignFamilyFather(new AssignFamilyParentInput
                {
                    ParentId = outOfRangePersonId,
                    FamilyId = salahFamilyId
                });
            });
        }

        [Fact]
        public void Should_Throw_AbpValidationException_If_Mother_Id_Is_Out_Of_Range()
        {
            var outOfRangePersonId = 0;

            var salahFamilyId = GetFamilyFromParentName("Madame", "Salah").Id;

            Should.Throw<AbpValidationException>(() =>
            {
                _familyAppService.AssignFamilyMother(new AssignFamilyParentInput
                {
                    ParentId = outOfRangePersonId,
                    FamilyId = salahFamilyId
                });
            });
        }

        [Fact]
        public void Should_Throw_AbpValidationException_If_Family_Id_Is_Out_Of_Range()
        {
            var OutOfRangeFamilyId = 0;
            var salahId = GetPerson("Mohamed", "Salah").Id;

            Should.Throw<AbpValidationException>(() =>
            {
                _familyAppService.AssignFamilyFather(new AssignFamilyParentInput
                {
                    ParentId = salahId,
                    FamilyId = OutOfRangeFamilyId
                });
            });
        }
        #endregion
    }
}
