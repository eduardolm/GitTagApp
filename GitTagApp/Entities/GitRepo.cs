using System.Collections.Generic;
using GitTagApp.Interfaces;

namespace GitTagApp.Entities
{
    public class GitRepo : IBaseEntity
    {
        public int Id { get; set; }
        public int GitRepoId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string HttpUrl { get; set; }

        public virtual int UserId { get; set; }
        
        public virtual IList<GitRepoTag> GitRepoTags { get; set; }
        public virtual User User { get; set; }
    }
}