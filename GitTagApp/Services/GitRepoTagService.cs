using GitTagApp.Entities;
using GitTagApp.Interfaces;
using GitTagApp.Services.GenericService;

namespace GitTagApp.Services
{
    public class GitRepoTagService : GenericService<GitRepoTag>, IGitRepoTagService
    {
        public GitRepoTagService(IGenericRepository<GitRepoTag> repository) : base(repository)
        {
        }
    }
}