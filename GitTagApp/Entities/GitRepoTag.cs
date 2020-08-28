namespace GitTagApp.Entities
{
    public class GitRepoTag
    {
        public int GitRepoId { get; set; }
        public GitRepo GitRepo { get; set; }
        
        public int TagId { get; set; }
        public Tag Tag { get; set; }
    }
}