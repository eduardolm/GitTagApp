using GitTagApp.Interfaces;

namespace GitTagApp.Entities
{
    public class GitRepoTag : IBaseEntity
    {
        public long GitRepoId { get; set; }
        public GitRepo GitRepo { get; set; }
        
        public long TagId { get; set; }
        public Tag Tag { get; set; }
        public long Id { get; set; }
    }
}