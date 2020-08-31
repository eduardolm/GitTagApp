using System.Collections.Generic;
using System.Linq;
using GitTagApp.Entities;
using GitTagApp.Interfaces;

namespace GitTagApp.Services
{
    public class RepoBuilder
    {
        private readonly IUserService _userService;

        private readonly ITagService _tagService;

        private readonly IGitRepoTagService _gitRepoTagService;

        public RepoBuilder(IUserService userService, ITagService tagService, IGitRepoTagService gitRepoTagService)
        {
            _userService = userService;
            _tagService = tagService;
            _gitRepoTagService = gitRepoTagService;
        }

        public GitRepo GetPayload(GitRepo repo)
        {
            repo.User = new User();
            repo.GitRepoTags = new List<GitRepoTag>();

            repo.User = (from u in _userService.GetAll()
                where u.Id == repo.UserId
                select u).FirstOrDefault();

            repo.GitRepoTags = (from rt in _gitRepoTagService.GetAll()
                join t in _tagService.GetAll()
                    on rt.TagId equals t.Id
                where rt.GitRepoId == repo.Id
                select rt).ToList();

            return repo;
        }
    }
}