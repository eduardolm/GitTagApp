using System.Collections.Generic;
using GitTagApp.Entities;

namespace GitTagApp.Tests.Unit.Entities
{
    public class GitRepoIdComparer : IEqualityComparer<GitRepo>
    {
        public bool Equals(GitRepo x, GitRepo y)
        {
            return x != null && x.Id == y.Id;
        }

        public int GetHashCode(GitRepo obj)
        {
            return obj.Id.GetHashCode();
        }
    }
}
