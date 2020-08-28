using System.Collections.Generic;

namespace GitTagApp.Entities
{
    public class Tag
    {
        public int Id { get; set; }
        public string Name { get; set; }
        
        public virtual IList<GitRepositoryTag> GitRepositoryTags { get; set; }
    }
}