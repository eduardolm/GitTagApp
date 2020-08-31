using GitTagApp.Entities;
using GitTagApp.Interfaces;
using GitTagApp.Repositories.Context;
using GitTagApp.Repositories.GenericRepository;

namespace GitTagApp.Repositories
{
    public class GitRepoTagRepository : GenericRepository<GitRepoTag>, IGitRepoTagRepository
    {
        private readonly MainContext _dbContext;
        public GitRepoTagRepository(MainContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }
        
        public void CreateRepoTag(GitRepoTag gitRepoTag)
        {
            var checkId = _dbContext.GitRepoTags.Find(gitRepoTag.GitRepoId, gitRepoTag.TagId);
            if (checkId != null) return;
            
            DetachLocal(_ => _.Id == gitRepoTag.GitRepoId);
            DetachLocal(_ => _.Id == gitRepoTag.TagId);
            _dbContext.GitRepoTags.Add(gitRepoTag);
            _dbContext.SaveChanges();
        }

        public void DeleteRepoTag(long tagId, long repoId)
        {
            var entity = _dbContext.GitRepoTags.Find(repoId, tagId);
            _dbContext.Remove(entity);
            _dbContext.SaveChanges();
        }
    }
}