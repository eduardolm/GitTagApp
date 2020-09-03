using System.Collections.Generic;
using GitTagApp.Entities;

namespace GitTagApp.Tests.Unit.Entities
{
    public class TagIdComparer : IEqualityComparer<Tag>
    {
        public bool Equals(Tag x, Tag y)
        {
            return x != null && x.Id == y.Id;
        }

        public int GetHashCode(Tag obj)
        {
            return obj.Id.GetHashCode();
        }
    }
}