using Foyer.EntityFramework;
using Foyer.Families;
using Foyer.People;
using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Foyer.Migrations.SeedData
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
            //Add some people            
            AddPersonIfNotExists("Mohamed", "Salah", Gender.Male, new DateTime(1992, 6, 15));
            AddPersonIfNotExists("Sadio", "Mané", Gender.Male, new DateTime(1992, 4, 10));
            AddPersonIfNotExists("Philippe", "Coutinho", Gender.Male, new DateTime(1992, 6, 12));
            AddPersonIfNotExists("Lionel", "Messi", Gender.Male, new DateTime(1987, 6, 24));
            AddPersonIfNotExists("Ousmane", "Dembélé", Gender.Male, new DateTime(1997, 5, 15));

            //Add some families
            
        }

        private void AddPersonIfNotExists(string firstName, string lastName, Gender gender, DateTime birthDate)
        {
            if (_context.People.Any(p => p.FirstName == firstName && p.LastName == lastName && p.Gender == gender && p.BirthDate == birthDate))
            {
                return;
            }

            _context.People.Add(new Person()
            {
                FirstName = firstName,
                LastName = lastName,
                Gender = gender,
                BirthDate = birthDate
            });

            _context.SaveChanges();
        }

        private void AddFamilyIfNotExists(string familyName, DateTime widingDate)
        {
            if (_context.Families.Any(p => p.FamilyName == familyName && p.WidingDate == widingDate))
            {
                return;
            }

            _context.Families.Add(new Family()
            {
                FamilyName = familyName,
                WidingDate = widingDate
            });

            _context.SaveChanges();
        }
    }
}
