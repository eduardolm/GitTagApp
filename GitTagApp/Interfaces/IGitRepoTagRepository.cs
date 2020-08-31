using GitTagApp.Entities;

namespace GitTagApp.Interfaces
{
    public interface IGitRepoTagRepository : IGenericRepository<GitRepoTag>
    {
        void CreateRepoTag(GitRepoTag gitRepoTag);
        void DeleteRepoTag(long tagId, long repoId);
    }
}