namespace GitTagApp.Entities
{
    public class GitRepositoryTag
    {
        public int GitRepositoryId { get; set; }
        public GitRepository GitRepository { get; set; }
        
        public int TagId { get; set; }
        public Tag Tag { get; set; }
    }
}