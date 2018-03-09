using Abp.Domain.Entities;
using Abp.Runtime.Validation;
using Foyer.Families;
using Foyer.Families.Dto;
using Foyer.People;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

        #region AssignPersonHeadOfFamily tests
        [Fact]
        public void Should_Assign_Person_Head_Of_Family()
        {
            var newPerson = UsingDbContext(Context => Context.People.Add(new Person
            {
                FirstName = "Lo",
                LastName = "Celso",
                Gender = Gender.Male
            }));

            var newFamily = UsingDbContext(Context => Context.Families.Add(new Family()));
            
            _familyAppService.AssignPersonHeadOfFamily(new PersonHeadOfFamilyInput
            {
                PersonId = newPerson.Id,
                FamilyId = newFamily.Id
            });

            UsingDbContext(context => 
            {
                var createdPerson = context.People.FirstOrDefault(p => p.Id == newPerson.Id);
                createdPerson.ShouldNotBeNull();
                createdPerson.FirstName.ShouldBe(newPerson.FirstName);

                var createdFamily = context.Families.FirstOrDefault(f => f.Id == newFamily.Id);
                createdFamily.ShouldNotBeNull();
                createdFamily.HusbandId.ShouldBe(createdPerson.Id);
            });
        }

        [Fact]
        public void Should_Not_Throw_Exception_If_Assigned_Person_Is_Already_Head_Of_Family()
        {
            var salahId = GetPerson("Mohamed", "Salah").Id;
            var salahFamilyId = GetFamilyFromHeadOfFamilyId(salahId).Id;

            Should.NotThrow(() => _familyAppService.AssignPersonHeadOfFamily(new PersonHeadOfFamilyInput
            {
                PersonId = salahId,
                FamilyId = salahFamilyId
            }));

            UsingDbContext(context =>
            {
                context.Families.FirstOrDefault(f => f.Id == salahFamilyId && f.HusbandId == salahId).ShouldNotBeNull();
            });
        }

        [Fact]
        public void Should_Not_Assign_Person_Head_Of_Family_If_Person_Does_Not_Exist()
        {
            var notExistingPersonId = GenerateNotExistingPersonId();

            var newFamily = UsingDbContext(Context => Context.Families.Add(new Family()));

            Should.Throw<EntityNotFoundException>(() =>
            {
                _familyAppService.AssignPersonHeadOfFamily(new PersonHeadOfFamilyInput
                {
                    PersonId = notExistingPersonId,
                    FamilyId = newFamily.Id
                });
            });
        }

        [Fact]
        public void Should_Throw_AbpValidationException_If_Person_Id_Is_Out_Of_Range()
        {
            var outOfRangePersonId = 0;

            var salahFamilyId = GetFamilyFromHeadOfFamilyName("Mohamed", "Salah").Id;

            Should.Throw<AbpValidationException>(() =>
            {
                _familyAppService.AssignPersonHeadOfFamily(new PersonHeadOfFamilyInput
                {
                    PersonId = outOfRangePersonId,
                    FamilyId = salahFamilyId
                });
            });
        }

        [Fact]
        public void Should_Not_Assign_Person_Head_Of_Family_If_Family_Does_Not_Exist()
        {
            var notExistingFamilyId = GenerateNotExistingFamilyId();

            var salahId = GetPerson("Mohamed", "Salah").Id;

            Should.Throw<EntityNotFoundException>(() =>
            {
                _familyAppService.AssignPersonHeadOfFamily(new PersonHeadOfFamilyInput
                {
                    PersonId = salahId,
                    FamilyId = notExistingFamilyId
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
                _familyAppService.AssignPersonHeadOfFamily(new PersonHeadOfFamilyInput
                {
                    PersonId = salahId,
                    FamilyId = OutOfRangeFamilyId
                });
            });
        }
        #endregion
    }
}
