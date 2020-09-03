using GitTagApp.Entities;

namespace GitTagApp.Interfaces
{
    public interface IGitRepoTagService : IGenericService<GitRepoTag>
    {
        GitRepoTag GetByIdRepoTag(long repoId, long tagId);
        string CreateRepoTag(GitRepoTag gitRepoTag);
        string DeleteRepoTag(long repoId, long tagId);
    }
}