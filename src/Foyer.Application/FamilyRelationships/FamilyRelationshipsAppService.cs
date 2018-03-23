using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using Abp.ObjectMapping;
using Foyer.Families;
using Foyer.People;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Foyer.FamilyRelationships
{
    public class FamilyRelationshipsAppService : IFamilyRelationshipsAppService
    {
        private readonly IRepository<Person> _personRepository;
        private readonly IRepository<Family> _familyRepository;
        private readonly IRepository<FamilyRelationship> _familyRelationshipsRepository;
        private readonly IFamilyManager _familyManager;
        private readonly IObjectMapper _objectMapper;
        private readonly IUnitOfWorkManager _unitOfWorkManager;

        public FamilyRelationshipsAppService(
            IRepository<Person> personRepository,
            IRepository<Family> familyRepository,
            IRepository<FamilyRelationship> familyRelationshipsRepository,
            IFamilyManager familyManager,
            IObjectMapper objectMapper,
            IUnitOfWorkManager unitOfWorkManager)
        {
            _personRepository = personRepository;
            _familyRepository = familyRepository;
            _familyRelationshipsRepository = familyRelationshipsRepository;
            _familyManager = familyManager;
            _objectMapper = objectMapper;
            _unitOfWorkManager = unitOfWorkManager;
        }

        public void AddFamilyMember(Family family, Person member, FamilyRelationship relationship)
        {
            //If the person is a parent, call AssignFamilyParents of app service because it will set parents in family entity ?

            //Find and update a relationship between the person and the parents,
            //first with the father then with the mother

            //If not exist create new relationship between the person and family parents.

            if (family != null && member != null)
            {
                relationship = new FamilyRelationship
                {
                    Family = family,
                    Person = member,

                };

                //return relationship;
            }

            //throw user friendly exception
        }

        public void DeleteFamilyMember(Family family, Person member)
        {
            //Delete all relationships Where(r.FamilyId == family.Id && (r.PersonId == member.Id || r.RelatedPersonId == member.Id)); 
            throw new NotImplementedException("Should delete all relationships between this person and all members of this family including the parents");
        }

        public void AddRelationship(FamilyRelationship relationship)
        {
            throw new NotImplementedException();
        }

        public void AddOrUpdateParentsRelationship(Family family, bool married)
        {
            //Both parents should be defined in order to add or update a relationship
            if (!(family.FatherId.HasValue && family.MotherId.HasValue))
            {
                return;
            }

            var parentsRelationshipType = married ? RelationshipType.Married : RelationshipType.Divorced;

            //Look for existing relationship even if deleted, should i do that ?
            using (_unitOfWorkManager.Current.DisableFilter(AbpDataFilters.SoftDelete))
            {
                var relationship = _familyRelationshipsRepository.GetAll()
                .Where(r => r.PersonId == family.FatherId && r.RelatedPersonId == family.MotherId)
                .Where(r => r.RelationshipType == RelationshipType.Married || r.RelationshipType == RelationshipType.Divorced)
                .FirstOrDefault();

                if (relationship == null)
                {
                    _familyRelationshipsRepository.Insert(new FamilyRelationship
                    {
                        RelationshipType = parentsRelationshipType,
                        FamilyId = family.Id,
                        PersonId = family.FatherId.Value,
                        PersonRole = PersonRole.Husband,
                        RelatedPersonId = family.MotherId.Value,
                        RelatedPersonRole = PersonRole.Wife
                    });
                }
                else
                {
                    relationship.RelationshipType = parentsRelationshipType;
                    relationship.FamilyId = family.Id;
                    relationship.IsDeleted = false;
                }
            }
        }

    }
}
