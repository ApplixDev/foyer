﻿using Abp.Domain.Entities;
using Abp.Runtime.Validation;
using Abp.UI;
using Foyer.Families;
using Foyer.Families.Dto;
using Foyer.People;
using Shouldly;
using System.Linq;
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

        #region Create

        [Fact]
        public void Should_Throw_AbpValidationException_If_Both_Parents_Ids_Are_Null()
        {
            Should.Throw<AbpValidationException>(() => _familyAppService.Create(new CreateFamilyDto()));
        }

        [Fact]
        public void Should_Throw_AbpValidationException_If_Same_Person_Is_Assigned_As_Father_And_As_Mother()
        {
            Should.Throw<AbpValidationException>(() => _familyAppService.Create(new CreateFamilyDto
            {
                FatherId = 1,
                MotherId = 1
            }));
        }

        [Fact]
        public void Should_Throw_UserFriendlyException_If_Family_Already_Exists()
        {
            var singleManId = GetUnmarriedMan().Id;
            var singleWomenId = GetUnmarriedWomen().Id;

            //Add family too make it exists
            UsingDbContext(Context =>
            {
                Context.Families.Add(new Family { FatherId = singleManId, MotherId = singleWomenId });
            });

            var familyInputWithBothParents = new CreateFamilyDto { FatherId = singleManId, MotherId = singleWomenId };

            Should.Throw<UserFriendlyException>(() =>
            {
                _familyAppService.Create(familyInputWithBothParents);
            })
            .Message.ShouldBe("This family already exists");
        }

        [Fact]
        public void Should_Throw_UserFriendlyException_If_Single_Mother_Family_Already_Exists()
        {
            var singleWomenId = GetUnmarriedWomen().Id;

            UsingDbContext(Context =>
            {
                Context.Families.Add(new Family { MotherId = singleWomenId });
            });

            var familyInputWithOnlyMother = new CreateFamilyDto { MotherId = singleWomenId };

            Should.Throw<UserFriendlyException>(() =>
            {
                _familyAppService.Create(familyInputWithOnlyMother);
            })
            .Message.ShouldBe("This family already exists");
        }

        [Fact]
        public void Should_Throw_UserFriendlyException_If_Single_Father_Family_Already_Exists()
        {
            var singleManId = GetUnmarriedMan().Id;

            UsingDbContext(Context =>
            {
                Context.Families.Add(new Family { FatherId = singleManId });
            });

            var familyInputWithOnlyFather = new CreateFamilyDto { FatherId = singleManId };

            Should.Throw<UserFriendlyException>(() =>
            {
                _familyAppService.Create(familyInputWithOnlyFather);
            })
            .Message.ShouldBe("This family already exists");
        }

        [Fact]
        public void Should_Throw_EntityNotFoundException_If_Family_Father_Do_Not_Exists()
        {
            Should.Throw<EntityNotFoundException>(() =>
            {
                _familyAppService.Create(new CreateFamilyDto
                {
                    FatherId = GenerateNotExistingPersonId()
                });
            });
        }

        [Fact]
        public void Should_Throw_EntityNotFoundException_If_Family_Mother_Do_Not_Exists()
        {
            Should.Throw<EntityNotFoundException>(() =>
            {
                _familyAppService.Create(new CreateFamilyDto
                {
                    MotherId = GenerateNotExistingPersonId()
                });
            });
        }

        [Fact]
        public void Should_Throw_UserFriendlyException_If_Family_Father_Is_Not_A_Male()
        {
            Should.Throw<UserFriendlyException>(() =>
            {
                _familyAppService.Create(new CreateFamilyDto
                {
                    FatherId = GetUnmarriedWomen().Id
                });
            })
            .Message.ShouldBe("The family father must be a male");
        }

        [Fact]
        public void Should_Throw_UserFriendlyException_If_Family_Mother_Is_Not_A_Female()
        {
            Should.Throw<UserFriendlyException>(() =>
            {
                _familyAppService.Create(new CreateFamilyDto
                {
                    MotherId = GetUnmarriedMan().Id
                });
            })
            .Message.ShouldBe("The family mother must be a female");
        }

        [Fact]
        public void Should_Create_New_Family()
        {
            var initialFamiliesCount = GetFamiliesCount();

            var newHusband = GetUnmarriedMan();
            var newWife = GetUnmarriedWomen();

            _familyAppService.Create(new CreateFamilyDto
            {
                FatherId = newHusband.Id,
                MotherId = newWife.Id
            });

            UsingDbContext(context =>
            {
                context.Families.Count().ShouldBe(initialFamiliesCount + 1);
                context.Families.FirstOrDefault(f => f.FatherId == newHusband.Id && f.MotherId == newWife.Id)
                .ShouldNotBeNull();
            });
        }

        [Fact]
        public void Should_Create_Single_Parent_Family_With_Father()
        {
            var initialFamiliesCount = GetFamiliesCount();
            var singleMan = GetUnmarriedMan();

            _familyAppService.Create(new CreateFamilyDto { FatherId = singleMan.Id });

            UsingDbContext(context =>
            {
                context.Families.Count().ShouldBe(initialFamiliesCount + 1);   
                context.Families.FirstOrDefault(f => f.FatherId == singleMan.Id && f.MotherId == null).ShouldNotBeNull();
            });
        }

        [Fact]
        public void Should_Create_Single_Parent_Family_With_Mother()
        {
            var initialFamiliesCount = GetFamiliesCount();
            var singleWoman = GetUnmarriedWomen();

            _familyAppService.Create(new CreateFamilyDto { MotherId = singleWoman.Id });

            UsingDbContext(context =>
            {
                context.Families.Count().ShouldBe(initialFamiliesCount + 1);
                context.Families.FirstOrDefault(f => f.MotherId == singleWoman.Id && f.FatherId == null).ShouldNotBeNull();
            });
        }

        #endregion

        #region Update
        [Fact]
        public void Should_Update_Family()
        {
            var existingFamilyId = GetFamilyFromParentId(1).Id;

            var updateFamilyDto = new UpdateFamilyDto
            {
                FamilyId = existingFamilyId,
                FatherId = 3,
                MotherId = 4,
                OtherDetails = "et ils eurent beaucoup d'enfants"
            };

            _familyAppService.Update(updateFamilyDto);

            UsingDbContext(context =>
            {
                var family = context.Families.FirstOrDefault(f => f.Id == existingFamilyId);
                family.ShouldNotBeNull();
                family.FatherId.ShouldBe(updateFamilyDto.FatherId);
                family.MotherId.ShouldBe(updateFamilyDto.MotherId);
                family.OtherDetails.ShouldBe(updateFamilyDto.OtherDetails);
            });
        }

        [Fact]
        public void Should_Throw_EntityNotFoundException_If_Family_Does_Not_Exists()
        {
            var notExistingFamilyId = GenerateNotExistingFamilyId();

            var notExistingFamily = new UpdateFamilyDto
            {
                FamilyId = notExistingFamilyId,
                FatherId = 1,
                MotherId = 2
            };

            Should.Throw<EntityNotFoundException>(() => _familyAppService.Update(notExistingFamily));
        }
        #endregion

        #region Delete
        [Fact]
        public void Should_Soft_Delete_Family()
        {
            var existingFamilyId = 1;
            var initialFamiliesCount = GetFamiliesCount();

            _familyAppService.Delete(new DeleteFamilyInput { FamilyId = existingFamilyId });

            UsingDbContext(context =>
            {
                context.Families.Count().ShouldBe(initialFamiliesCount);
                context.Families.First(p => p.Id == existingFamilyId).IsDeleted.ShouldBeTrue();
            });
        }

        [Fact]
        public void Should_Not_Delete_Family_If_Family_Does_Not_Exist()
        {
            var initialFamiliesCount = GetFamiliesCount();
            var notExistingFamilyId = GenerateNotExistingFamilyId();

            Should.Throw<UserFriendlyException>(() =>
            {
                _familyAppService.Delete(new DeleteFamilyInput { FamilyId = notExistingFamilyId });
            }).Message.ShouldBe("This family does not exists");

            GetFamiliesCount().ShouldBe(initialFamiliesCount);
        }
        #endregion

        #region Get
        [Fact]
        public void Should_Throw_EntityNotFoundException_If_Input_Family_Does_Not_Exist()
        {
            var notExistingFamilyId = GenerateNotExistingFamilyId();

            Should.Throw<EntityNotFoundException>(() => _familyAppService.Get(new GetFamilyInput
            {
                FamilyId = notExistingFamilyId
            }));
        }

        [Fact]
        public void Should_Return_Found_FamilyDto()
        {
            var existingFamilyId = GetFamilyFromParentId(1).Id;

            var output = _familyAppService.Get(new GetFamilyInput { FamilyId = existingFamilyId });

            output.ShouldBeOfType<FamilyDto>();
            output.Id.ShouldBe(existingFamilyId);
        }
        #endregion

        #region Get all families
        [Fact]
        public async Task GetAllFamilies_Test()
        {
            var output = await _familyAppService.GetAllFamilies();

            output.Families.Count().ShouldBeGreaterThan(0);
        }
        #endregion

        #region Assign family parents
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
            var existingPersonId = 1;
            var existingFamilyId = GetFamilyFromParentId(existingPersonId).Id;

            Should.NotThrow(() => _familyAppService.AssignFamilyFather(new AssignFamilyParentInput
            {
                ParentId = existingPersonId,
                FamilyId = existingFamilyId
            }));

            UsingDbContext(context =>
            {
                context.Families.FirstOrDefault(f => f.Id == existingFamilyId && f.FatherId == existingPersonId).ShouldNotBeNull();
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
            var existingPersonId = 1;

            Should.Throw<EntityNotFoundException>(() =>
            {
                _familyAppService.AssignFamilyFather(new AssignFamilyParentInput
                {
                    FamilyId = notExistingFamilyId,
                    ParentId = existingPersonId
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
            var existingPersonId = 1;

            Should.Throw<AbpValidationException>(() =>
            {
                _familyAppService.AssignFamilyFather(new AssignFamilyParentInput
                {
                    FamilyId = OutOfRangeFamilyId,
                    ParentId = existingPersonId,
                });
            });
        }
        #endregion
    }
}
