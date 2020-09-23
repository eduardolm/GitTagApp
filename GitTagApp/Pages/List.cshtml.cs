using System.Collections.Generic;
using System.Threading.Tasks;
using GitTagApp.Entities;
using GitTagApp.Interfaces;
using GitTagApp.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using Octokit;
using Octokit.Internal;

namespace GitTagApp.Pages
{
    public class ListModel : PageModel
    {
        public IReadOnlyList<Repository> Repositories { get; set; }
        public IReadOnlyList<Repository> StarredRepos { get; set; }
        public List<GitRepo> GitRepos { get; set; }
        
        private readonly ILogger<ErrorModel> _logger;

        private readonly IGitRepoService _repoService;

        private readonly IUserService _userService;

        private readonly ITagService _tagService;

        private readonly IGitRepoTagService _gitRepoTagService;

        public ListModel(ILogger<ErrorModel> logger, IGitRepoService repoService, IUserService userService, ITagService tagService, IGitRepoTagService gitRepoTagService)
        {
            _logger = logger;
            _repoService = repoService;
            _userService = userService;
            _tagService = tagService;
            _gitRepoTagService = gitRepoTagService;
        }

        public async Task OnGet()
        {
            if (User.Identity.IsAuthenticated)
            {
                var outRepos = new List<GitRepo>();
                var gitName = string.Empty;
                string accessToken = await HttpContext.GetTokenAsync("access_token");
                var github = new GitHubClient(new ProductHeaderValue("AspNetCoreGitHubAuth"), 
                    new InMemoryCredentialStore(new Credentials(accessToken)));
            
                Repositories = await github.Repository.GetAllForCurrent();
            
                StarredRepos = await github.Activity.Starring.GetAllForCurrent();
            
                var tempUser = await github.User.Get(Repositories[0].Owner.Login);
                gitName = tempUser.Name;
                            
                var dbUser = new Entities.User
                {
                    Id = Repositories[0].Owner.Id,
                    Name = gitName
                };
                _userService.Create(dbUser);
                
                foreach (var repo in StarredRepos)
                {
                    var dbRepo = new GitRepo
                    {
                        Id = repo.Id,
                        Name = repo.Name,
                        Description = repo.Description,
                        HttpUrl = repo.HtmlUrl,
                        UserId = dbUser.Id,
                    };
                    _repoService.Create(dbRepo);
                    
                    var repoBuilder = new RepoBuilder(_userService, _tagService, _gitRepoTagService);
                    var outputRepo = repoBuilder.GetPayload(dbRepo);
                    outRepos.Add(outputRepo);
                }

                GitRepos = outRepos;
            }
        }
    }
}
