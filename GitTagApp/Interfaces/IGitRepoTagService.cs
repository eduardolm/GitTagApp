using GitTagApp.Entities;

namespace GitTagApp.Interfaces
{
    public interface IGitRepoTagService : IGenericService<GitRepoTag>
    {
        new string CreateRepoTag(GitRepoTag gitRepoTag);
    }
}