using Abp.Domain.Services;
using Foyer.People;

namespace Foyer.Families
{
    public interface IFamilyManager : IDomainService
    {
        void AssignFamilyParents(Family family, Person father, Person mother);
        void AssignFamilyFather(Family family, Person father);
        void AssignFamilyMother(Family family, Person mother);

        /// <summary>
        /// Check if any family with the defined parents ids exists.
        /// Both parents ids are used to find matching family even if one or both of those ids are null.
        /// </summary>
        /// <param name="family"></param>
        /// <returns></returns>
        bool ParentsFamilyExists(Family family);
    }
}
