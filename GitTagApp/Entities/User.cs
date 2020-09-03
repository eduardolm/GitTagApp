using System.Collections.Generic;
using GitTagApp.Interfaces;

namespace GitTagApp.Entities
{
    public class User : IBaseEntity
    {
        public long Id { get; set; }
        public string Name { get; set; }
        
        public virtual ICollection<GitRepo> GitRepos { get; set; }
    }
}