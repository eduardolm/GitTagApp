using System.Linq;
using GitTagApp.Entities;
using GitTagApp.Interfaces;
using GitTagApp.Repositories.Context;
using GitTagApp.Services.GenericService;

namespace GitTagApp.Services
{
    public class GitRepoService : GenericService<GitRepo>, IGitRepoService
    {
        public GitRepoService(IGenericRepository<GitRepo> repository, MainContext dbContext) : base(repository)
        {
        }

        // public new void Create(GitRepo gitRepo)
        // {
        //     var storedRepos = (
        //         from r in _repository.GetAll()
        //         where gitRepo.GitRepoId == r.GitRepoId
        //         select r);
        //
        //     if (storedRepos.Count() != 0) return;
        //     _repository.Create(gitRepo);
        // }
    }
}