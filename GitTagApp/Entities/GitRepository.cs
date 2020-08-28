using System.Collections.Generic;

namespace GitTagApp.Entities
{
    public class GitRepository
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string HttpUrl { get; set; }

        public virtual int UserId { get; set; }
        
        public virtual IList<GitRepositoryTag> GitRepositoryTags { get; set; }
        public virtual User User { get; set; }
    }
}