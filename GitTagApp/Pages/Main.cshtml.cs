using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Castle.Core.Internal;
using GitTagApp.Entities;
using GitTagApp.Interfaces;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using Octokit;
using Octokit.Internal;

namespace GitTagApp.Pages
{
    public class MainModel : PageModel
    {
        public string RequestId { get; set; }

        private readonly ILogger<ErrorModel> _logger;

        public IReadOnlyList<Repository> Repositories { get; set; }
        public IReadOnlyList<Repository> StarredRepos { get; set; }
        private readonly IGitRepoTagService _gitRepoTagService;

        public MainModel(ILogger<ErrorModel> logger, IGitRepoTagService gitRepoTagService)
        {
            _logger = logger;
            _gitRepoTagService = gitRepoTagService;
        }

        public async Task OnGet()
        {
            if (User.Identity.IsAuthenticated)
            {
                var removeTag = new List<GitRepoTag>();
                string accessToken = await HttpContext.GetTokenAsync("access_token");
                var github = new GitHubClient(new ProductHeaderValue("AspNetCoreGitHubAuth"),
                    new InMemoryCredentialStore(new Credentials(accessToken)));

                Repositories = await github.Repository.GetAllForCurrent();

                StarredRepos = await github.Activity.Starring.GetAllForCurrent();

                var isStillStarred = _gitRepoTagService.GetAll().ToList();

                removeTag.AddRange(
                    (from rt in isStillStarred
                        join sr in StarredRepos on rt.GitRepoId !equals sr.Id
                        select rt).ToList());

                if (!removeTag.IsNullOrEmpty())
                {
                    foreach (var item in removeTag)
                    {
                        var result = _gitRepoTagService.DeleteRepoTag(item.GitRepoId, item.TagId);
                    }
                }
            }
        }
    }
}
