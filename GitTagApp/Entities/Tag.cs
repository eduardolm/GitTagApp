using System.Collections.Generic;
using GitTagApp.Interfaces;

namespace GitTagApp.Entities
{
    public class Tag : IBaseEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }
        
        public virtual IList<GitRepositoryTag> GitRepositoryTags { get; set; }
    }
}