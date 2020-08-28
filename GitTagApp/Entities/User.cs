using System.Collections.Generic;
using GitTagApp.Interfaces;

namespace GitTagApp.Entities
{
    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; }
        
        public virtual ICollection<GitRepository> GitRepositories { get; set; }
    }
}