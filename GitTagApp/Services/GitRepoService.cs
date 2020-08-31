using GitTagApp.Entities;
using GitTagApp.Interfaces;
using GitTagApp.Repositories.Context;
using GitTagApp.Services.GenericService;

namespace GitTagApp.Services
{
    public class GitRepoService : GenericService<GitRepo>, IGitRepoService
    {
        public GitRepoService(IGenericRepository<GitRepo> repository) : base(repository)
        {
        }
    }
}