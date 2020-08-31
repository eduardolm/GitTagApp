using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Castle.Core.Internal;
using GitTagApp.Entities;
using GitTagApp.Interfaces;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Razor.Language;
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
        
        [ViewData]
        public string Message { get; set; }
        public bool IsPostSuccess { get; set; }
        
        private readonly ILogger<ErrorModel> _logger;

        private readonly IGitRepoService _repoService;

        private readonly IUserService _userService;

        private readonly ITagService _tagService;

        private readonly IGitRepoTagService _gitRepoTagService;

        public TagModel(ILogger<ErrorModel> logger, IGitRepoService repoService, IUserService userService, ITagService tagService, IGitRepoTagService gitRepoTagService)
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

        public void OnPostCreate()
        {
            IsPostSuccess = false;
            
            var tag = new Tag();
            tag.Name = Request.Form["Tag.Name"];
            
            var dbTags = (from t in _tagService.GetAll()
                where t.Name == tag.Name
                select t).ToList();
            
            if (!dbTags.Any()) _tagService.Create(tag);
            
            var relatedTag = (from t in _tagService.GetAll()
                where t.Name == tag.Name
                select t).Single();
            
            var gitRepoTag = new GitRepoTag();
            gitRepoTag.GitRepoId = Convert.ToInt64(Request.Form["starredRepoId"]);
            gitRepoTag.TagId = relatedTag.Id;
            Message = _gitRepoTagService.CreateRepoTag(gitRepoTag);
        }

        public void OnPostUpdate()
        {
            IsPostSuccess = false;
            var newTag = new Tag();
            
            newTag.Name = Request.Form["Tag.Name"]; // New tag name (from text input)
            var selectedTag = Convert.ToInt64(Request.Form["tagId"]); // Selected from select (tag to be updated)
            newTag.Id = selectedTag;
            var result = _tagService.Update(newTag);

            if (!result.IsNullOrEmpty()) IsPostSuccess = true;
        }

        public void OnPostDelete(long id)
        {
            IsPostSuccess = false;

            var result = _tagService.Delete(Convert.ToInt64(Request.Form["tagId"]));

            if (!result.IsNullOrEmpty()) IsPostSuccess = true;
        }
    }
}
