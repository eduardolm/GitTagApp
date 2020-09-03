using System.Collections.Generic;
using GitTagApp.Entities;

namespace GitTagApp.Tests.Unit.Entities
{
    public class UserIdComparer : IEqualityComparer<User>
    {
        public bool Equals(User x, User y)
        {
            return x != null && x.Id == y.Id;
        }

        public int GetHashCode(User obj)
        {
            return obj.Id.GetHashCode();
        }
    }
}