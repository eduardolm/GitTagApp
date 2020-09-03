using System.Collections.Generic;
using GitTagApp.Entities;

namespace GitTagApp.Tests.Unit.Entities
{

    public class GitRepoTagIdComparer : IEqualityComparer<GitRepoTag>
    {
        public bool Equals(GitRepoTag x, GitRepoTag y)
        {
            return x != null && x.Id == y.Id;
        }

        public int GetHashCode(GitRepoTag obj)
        {
            return obj.Id.GetHashCode();
        }
    }
}
