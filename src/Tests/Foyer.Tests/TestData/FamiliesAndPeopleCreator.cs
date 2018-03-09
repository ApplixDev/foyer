using Foyer.EntityFramework;
using Foyer.Families;
using Foyer.FamilyRelationships;
using Foyer.People;
using System;
using System.Linq;

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
            var salah = AddPersonIfNotExists("Mohamed", "Salah", Gender.Male, new DateTime(1992, 6, 15), "Egypt");
            var salahWife = AddPersonIfNotExists("Madame", "Salah", Gender.Female, new DateTime(1992, 6, 15), "Egypt");

            var mane = AddPersonIfNotExists("Sadio", "Mané", Gender.Male, new DateTime(1992, 4, 10), "Sénégal");
            var maneWife = AddPersonIfNotExists("Madame", "Mané", Gender.Female, new DateTime(1992, 4, 10), "Sénégal");

            var coutinho = AddPersonIfNotExists("Philippe", "Coutinho", Gender.Male, new DateTime(1992, 6, 12), "Brésil");
            var coutinhoWife = AddPersonIfNotExists("Madame", "Coutinho", Gender.Female, new DateTime(1992, 6, 12), "Brésil");

            var messi = AddPersonIfNotExists("Lionel", "Messi", Gender.Male, new DateTime(1987, 6, 24), "Argentine");
            var messiWife = AddPersonIfNotExists("Madame", "Messi", Gender.Female, new DateTime(1987, 6, 24), "Argentine");

            var dembele = AddPersonIfNotExists("Ousmane", "Dembélé", Gender.Male, new DateTime(1997, 5, 15), "France");
            var dembeleWife = AddPersonIfNotExists("Madame", "Dembélé", Gender.Male, new DateTime(1997, 5, 15), "France");

            //Add soft deleted person
            var zidane = AddPersonIfNotExists("Zindine", "Zidane", Gender.Male, new DateTime(1972, 6, 23), "Marseille", isDeleted: true);

            //Add some families
            var salahFamily = AddFamilyIfNotExists(salah, salahWife, new DateTime(2012, 6, 15));
            var maneFamily = AddFamilyIfNotExists(mane, maneWife, new DateTime(2012, 4, 10));
            var coutinhoFamily = AddFamilyIfNotExists(coutinho, coutinhoWife, new DateTime(2012, 6, 12));
            var messiFamily = AddFamilyIfNotExists(messi, messiWife, new DateTime(2007, 6, 24));
            var dembeleFamily = AddFamilyIfNotExists(dembele, dembeleWife, new DateTime(2017, 5, 15));

            //Add some marriage relationships
            AddMarriageRelationshipIfNotExists(salahFamily, salah, salahWife);
            AddMarriageRelationshipIfNotExists(maneFamily, mane, maneWife);
            AddMarriageRelationshipIfNotExists(coutinhoFamily, coutinho, coutinhoWife);
            AddMarriageRelationshipIfNotExists(messiFamily, messi, messiWife);
            AddMarriageRelationshipIfNotExists(dembeleFamily, dembele, dembeleWife);
        }

        /// <summary>
        /// Add Person entity if it does not Exist
        /// </summary>
        /// <param name="firstName">First name</param>
        /// <param name="lastName">Last name</param>
        /// <param name="gender">Gender</param>
        /// <param name="birthDate">Birth date</param>
        /// <param name="birthPlace">Birth place</param>
        /// <param name="isDeleted">Is this person deleted</param>
        /// <returns></returns>
        private Person AddPersonIfNotExists(string firstName, string lastName, Gender gender, DateTime birthDate, string birthPlace, bool isDeleted = false)
        {
            var person = _context.People.FirstOrDefault(p => p.FirstName == firstName && p.LastName == lastName && p.Gender == gender && p.BirthDate == birthDate);

            if (person == null)
            {
                person = _context.People.Add(new Person()
                {
                    FirstName = firstName,
                    LastName = lastName,
                    Gender = gender,
                    BirthDate = birthDate,
                    BirthPlace = birthPlace,
                    IsDeleted = isDeleted
                });

                _context.SaveChanges();
            }

            return person;
        }

        private Family AddFamilyIfNotExists(Person husband, Person wife, DateTime widingDate)
        {
            var family = _context.Families.FirstOrDefault(p => p.Husband.Id == husband.Id && p.WidingDate == widingDate);

            if (family == null)
            {
                family = _context.Families.Add(new Family()
                {
                    Husband = husband,
                    FamilyName = husband.LastName,
                    WidingDate = widingDate
                });
                
                _context.SaveChanges();
            }

            return family;
        }

        private void AddMarriageRelationshipIfNotExists(Family family, Person husband, Person wife)
        {
            if (_context.FamilyRelationships.Any(r => r.Person.Id == husband.Id && r.RelatedPerson.Id == wife.Id && r.RelationshipType == RelationshipType.Maried))
            {
                return;
            }

            var relationship = new FamilyRelationship()
            {
                Family = family,
                Person = husband,
                RelatedPerson = wife,
                PersonRole = PersonRole.Husband,
                RelatedPersonRole = PersonRole.Wife,
                RelationshipType = RelationshipType.Maried
            };

            _context.FamilyRelationships.Add(relationship);
            _context.SaveChanges();
        }
    }
}
