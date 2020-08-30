using GitTagApp.Entities;
using GitTagApp.Interfaces;
using GitTagApp.Repositories.Context;
using GitTagApp.Repositories.GenericRepository;

namespace GitTagApp.Repositories
{
    public class GitRepoTagRepository : GenericRepository<GitRepoTag>, IGitRepoTagRepository
    {
        public GitRepoTagRepository(MainContext dbContext) : base(dbContext)
        {
        }
    }
}