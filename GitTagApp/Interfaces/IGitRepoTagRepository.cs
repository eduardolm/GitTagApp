using GitTagApp.Entities;

namespace GitTagApp.Interfaces
{
    public interface IGitRepoTagRepository : IGenericRepository<GitRepoTag>
    {
        GitRepoTag GetByIdRepoTag(long repoId, long tagId);
        void CreateRepoTag(GitRepoTag gitRepoTag);
        void DeleteRepoTag(long repoId, long tagId);
    }
}