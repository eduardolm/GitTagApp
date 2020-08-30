using GitTagApp.Entities;
using GitTagApp.Interfaces;
using GitTagApp.Repositories.Context;
using GitTagApp.Repositories.GenericRepository;

namespace GitTagApp.Repositories
{
    public class GitRepoRepository : GenericRepository<GitRepo>, IGitRepoRepository
    {
        public GitRepoRepository(MainContext dbContext) : base(dbContext)
        {
        }
    }
}