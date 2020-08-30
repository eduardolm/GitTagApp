using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GitTagApp.Entities;
using GitTagApp.Interfaces;
using GitTagApp.Repositories.Context;
using GitTagApp.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;
using Octokit;
using Octokit.Internal;

namespace GitTagApp.Pages
{
    public class TagModel : PageModel
    {
        public IReadOnlyList<Repository> Repositories { get; set; }
        public IReadOnlyList<Repository> StarredRepos { get; set; }
        
        [BindProperty]
        public Tag Tag { get; set; }
        public List<SelectListItem> RepoOptions { get; set; }
        public List<SelectListItem> TagOptions { get; set; }
        
        private readonly ILogger<ErrorModel> _logger;

        private readonly IGitRepoService _repoService;

        private readonly IUserService _userService;

        private readonly ITagService _tagService;

        public TagModel(ILogger<ErrorModel> logger, IGitRepoService repoService, IUserService userService, ITagService tagService)
        {
            _logger = logger;
            _repoService = repoService;
            _userService = userService;
            _tagService = tagService;
        }

        public async Task OnGet()
        {
            if (User.Identity.IsAuthenticated)
            {
                var gitName = string.Empty;
                string accessToken = await HttpContext.GetTokenAsync("access_token");
                var github = new GitHubClient(new ProductHeaderValue("AspNetCoreGitHubAuth"), 
                    new InMemoryCredentialStore(new Credentials(accessToken)));
            
                Repositories = await github.Repository.GetAllForCurrent();
            
                StarredRepos = await github.Activity.Starring.GetAllForCurrent();

                RepoOptions = StarredRepos.Select(x =>
                    new SelectListItem
                    {
                        Value = x.Id.ToString(),
                        Text = x.Name
                    }).ToList();

                TagOptions = _tagService.GetAll().Select(x =>
                    new SelectListItem
                    {
                        Value = x.Id.ToString(),
                        Text = x.Name
                    }).ToList();
            }
        }

        public void OnPost()
        {
            var tag = new Tag();
            tag.Name = Request.Form["Tag.Name"];
            _tagService.Create(tag);
            
            var gitRepoTag = new GitRepoTag();
            gitRepoTag.GitRepoId = Convert.ToInt64(Request.Form["starredRepoId"]);
            gitRepoTag.TagId = tag.Id;
            

        }
    }
}
