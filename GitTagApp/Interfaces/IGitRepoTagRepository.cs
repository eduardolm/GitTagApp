using GitTagApp.Entities;

namespace GitTagApp.Interfaces
{
    public interface IGitRepoTagRepository : IGenericRepository<GitRepoTag>
    {
        new void CreateRepoTag(GitRepoTag gitRepoTag);
    }
}