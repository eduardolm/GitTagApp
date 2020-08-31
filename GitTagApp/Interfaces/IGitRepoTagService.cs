using GitTagApp.Entities;

namespace GitTagApp.Interfaces
{
    public interface IGitRepoTagService : IGenericService<GitRepoTag>
    {
        string CreateRepoTag(GitRepoTag gitRepoTag);
        string DeleteRepoTag(long tagId, long repoId);
    }
}