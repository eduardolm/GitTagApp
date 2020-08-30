using System.Collections.Generic;
using GitTagApp.Interfaces;

namespace GitTagApp.Entities
{
    public class Tag : IBaseEntity
    {
        public long Id { get; set; }
        public string Name { get; set; }
        
        public virtual IList<GitRepoTag> GitRepoTags { get; set; }
    }
}