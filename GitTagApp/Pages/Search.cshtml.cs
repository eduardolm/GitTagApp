using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GitTagApp.Entities;
using GitTagApp.Interfaces;
using GitTagApp.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using Octokit;
using Octokit.Internal;
using User = GitTagApp.Entities.User;

namespace GitTagApp.Pages
{
    public class SearchModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        
        private readonly IUserService _userService;
        
        private readonly ITagService _tagService;

        private readonly IGitRepoTagService _gitRepoTagService;
        public IReadOnlyList<Repository> Repositories { get; set; }
        public IReadOnlyList<Repository> StarredRepos { get; set; }
        public User UserEntity { get; set; }
        public List<GitRepo> GitRepos { get; set; }

        [BindProperty]
        public Tag Tag { get; set; }

        public SearchModel(ILogger<IndexModel> logger, IGitRepoService gitRepoService, ITagService tagService, IGitRepoTagService gitRepoTagService, IUserService userService)
        {
            _logger = logger;
            _tagService = tagService;
            _gitRepoTagService = gitRepoTagService;
            _userService = userService;
        }

        public void OnGet()
        {
        }

        public async Task OnPost()
        {
            if (User.Identity.IsAuthenticated)
            {
                var gitName = string.Empty;
                string accessToken = await HttpContext.GetTokenAsync("access_token");
                var github = new GitHubClient(new ProductHeaderValue("AspNetCoreGitHubAuth"), 
                    new InMemoryCredentialStore(new Credentials(accessToken)));
                
                Repositories = await github.Repository.GetAllForCurrent();
            
                StarredRepos = await github.Activity.Starring.GetAllForCurrent();
                
                var tempUser = await github.User.Get(Repositories[0].Owner.Login);
                gitName = tempUser.Name;
                            
                UserEntity = new User
                {
                    Id = Repositories[0].Owner.Id,
                    Name = gitName
                };
            }
            var outRepos = new List<GitRepo>();
            var stringToSearch = Request.Form["Tag.Name"];
            var returnRepos = (from gt in _gitRepoTagService.GetAll()
                join t in _tagService.GetAll()
                    on gt.TagId equals t.Id
                where t.Name.ToLower().Contains(stringToSearch.ToString().ToLower())
                select gt).ToList();

            var tempRepo = (from r in StarredRepos
                join gt in returnRepos
                    on r.Id equals gt.GitRepoId
                select r).ToList();

            foreach (var repo in tempRepo)
            {
                var dbRepo = new GitRepo
                {
                    Id = repo.Id,
                    Name = repo.Name,
                    Description = repo.Description,
                    HttpUrl = repo.Url,
                    UserId = UserEntity.Id,
                };
                    
                var repoBuilder = new RepoBuilder(_userService, _tagService, _gitRepoTagService);
                var outputRepo = repoBuilder.GetPayload(dbRepo);
                outRepos.Add(outputRepo);
            }

            GitRepos = outRepos;
        }
    }
}
