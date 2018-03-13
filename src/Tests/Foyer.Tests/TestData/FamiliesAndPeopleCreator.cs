using Foyer.EntityFramework;
using Foyer.Families;
using Foyer.FamilyRelationships;
using Foyer.People;
using System;
using System.Data.Entity.Migrations;

namespace Foyer.Tests.TestData
{
    public class FamiliesAndPeopleCreator
    {
        private readonly FoyerDbContext _context;

        public FamiliesAndPeopleCreator(FoyerDbContext context)
        {
            _context = context;
        }

        public void Create()
        {
            //Add some couples of people         
            var salah = AddOrUpdatePerson("Mohamed", "Salah", Gender.Male, new DateTime(1992, 6, 15), "Egypt");
            var salahWife = AddOrUpdatePerson("Madame", "Salah", Gender.Female, new DateTime(1992, 6, 15), "Egypt");

            var mane = AddOrUpdatePerson("Sadio", "Mané", Gender.Male, new DateTime(1992, 4, 10), "Sénégal");
            var maneWife = AddOrUpdatePerson("Madame", "Mané", Gender.Female, new DateTime(1992, 4, 10), "Sénégal");

            var coutinho = AddOrUpdatePerson("Philippe", "Coutinho", Gender.Male, new DateTime(1992, 6, 12), "Brésil");
            var coutinhoWife = AddOrUpdatePerson("Madame", "Coutinho", Gender.Female, new DateTime(1992, 6, 12), "Brésil");

            var messi = AddOrUpdatePerson("Lionel", "Messi", Gender.Male, new DateTime(1987, 6, 24), "Argentine");
            var messiWife = AddOrUpdatePerson("Madame", "Messi", Gender.Female, new DateTime(1987, 6, 24), "Argentine");

            var dembele = AddOrUpdatePerson("Ousmane", "Dembélé", Gender.Male, new DateTime(1997, 5, 15), "France");
            var dembeleWife = AddOrUpdatePerson("Madame", "Dembélé", Gender.Male, new DateTime(1997, 5, 15), "France");

            //Add soft deleted person
            var zidane = AddOrUpdatePerson("Zindine", "Zidane", Gender.Male, new DateTime(1972, 6, 23), "Marseille", isDeleted: true);

            //Add some families
            var salahFamily = AddOrUpdateFamily(salah, salahWife, new DateTime(2012, 6, 15));
            var maneFamily = AddOrUpdateFamily(mane, maneWife, new DateTime(2012, 4, 10));
            var coutinhoFamily = AddOrUpdateFamily(coutinho, coutinhoWife, new DateTime(2012, 6, 12));
            var messiFamily = AddOrUpdateFamily(messi, messiWife, new DateTime(2007, 6, 24));
            var dembeleFamily = AddOrUpdateFamily(dembele, dembeleWife, new DateTime(2017, 5, 15));

            //Add some marriage relationships
            AddOrUpdateMarriageRelationship(salahFamily, salah, salahWife);
            AddOrUpdateMarriageRelationship(maneFamily, mane, maneWife);
            AddOrUpdateMarriageRelationship(coutinhoFamily, coutinho, coutinhoWife);
            AddOrUpdateMarriageRelationship(messiFamily, messi, messiWife);
            AddOrUpdateMarriageRelationship(dembeleFamily, dembele, dembeleWife);
        }

        private Person AddOrUpdatePerson(string firstName, string lastName, Gender gender, DateTime birthDate, string birthPlace, bool isDeleted = false)
        {
            var person = new Person
            {
                FirstName = firstName,
                LastName = lastName,
                Gender = gender,
                BirthDate = birthDate,
                BirthPlace = birthPlace,
                IsDeleted = isDeleted
            };

            _context.People.AddOrUpdate(p => new { p.FirstName, p.LastName, p.Gender, p.BirthDate }, person);
            _context.SaveChanges();

            return person;
        }

        private Family AddOrUpdateFamily(Person husband, Person wife, DateTime? widingDate = null, DateTime? divorceDate = null)
        {
            var family = new Family
            {
                FatherId = husband.Id,
                MotherId = wife.Id,
                WidingDate = widingDate,
                DivorceDate = divorceDate
            };

            _context.Families.AddOrUpdate(f => new { f.FatherId, f.MotherId }, family);
            _context.SaveChanges();

            return family;
        }

        private void AddOrUpdateMarriageRelationship(Family family, Person husband, Person wife)
        {
            var relationship = new FamilyRelationship()
            {
                FamilyId = family.Id,
                PersonId = husband.Id,
                RelatedPersonId = wife.Id,
                PersonRole = PersonRole.Husband,
                RelatedPersonRole = PersonRole.Wife,
                RelationshipType = RelationshipType.Maried
            };

            _context.FamilyRelationships.AddOrUpdate(r => new { r.PersonId, r.RelatedPersonId }, relationship);
            _context.SaveChanges();
        }
    }
}
