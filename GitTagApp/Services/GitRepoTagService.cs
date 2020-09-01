using GitTagApp.Entities;
using GitTagApp.Interfaces;
using GitTagApp.Services.GenericService;

namespace GitTagApp.Services
{
    public class GitRepoTagService : GenericService<GitRepoTag>, IGitRepoTagService
    {
        private readonly IGitRepoTagRepository _repository;
        public GitRepoTagService(IGitRepoTagRepository repository) : base(repository)
        {
            _repository = repository;
        }

        public GitRepoTag GetByIdRepoTag(long repoId, long tagId)
        {
            return _repository.GetByIdRepoTag(repoId, tagId);
        }

        public string CreateRepoTag(GitRepoTag gitRepoTag)
        {
            _repository.CreateRepoTag(gitRepoTag);
            return "Item successfully created";
        }

        public string DeleteRepoTag(long tagId, long repoId)
        {
            _repository.DeleteRepoTag(tagId, repoId);
            return "Item successfully deleted.";
        }
        
    }
}