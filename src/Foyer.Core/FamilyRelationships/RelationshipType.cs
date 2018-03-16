using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Foyer.FamilyRelationships
{
    public enum RelationshipType : byte
    {
        Married = 1,
        Divorced = 2,
        ParentChild = 3,
        BrotherSister = 4
    }
}
