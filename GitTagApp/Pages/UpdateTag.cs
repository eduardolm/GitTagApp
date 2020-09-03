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
using Microsoft.Extensions.Logging;
using Octokit;
using Octokit.Internal;

namespace GitTagApp.Pages
{
    public class UpdateTagModel : PageModel
    {
        public IReadOnlyList<Repository> Repositories { get; set; }
        public IReadOnlyList<Repository> StarredRepos { get; set; }
        public IReadOnlyList<SelectListItem> RepoOptions { get; set; }

        [BindProperty]
        public string RepoId { get; set; }
        
        [BindProperty]
        public string RepoTag { get; set; }

        [BindProperty]
        public Tag Tag { get; set; }

        [ViewData]
        public string Message { get; set; }
        public bool IsPostSuccess { get; set; }
        
        private readonly ILogger<ErrorModel> _logger;

        private readonly ITagService _tagService;

        private readonly IGitRepoTagService _gitRepoTagService;

        public UpdateTagModel(ILogger<ErrorModel> logger, ITagService tagService, IGitRepoTagService gitRepoTagService)
        {
            _logger = logger;
            _tagService = tagService;
            _gitRepoTagService = gitRepoTagService;
        }

        public async Task OnGet()
        {
            if (User.Identity.IsAuthenticated)
            {
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
            }
        }

        public void OnPostUpdate()
        {
            IsPostSuccess = false;
            var newTag = new Tag();
            var repoId = Convert.ToInt64(Request.Form["RepoId"]);

            var repoTagItem = (from rt in _gitRepoTagService.GetAll()
                where rt.GitRepoId == repoId
                select rt).ToList();
            
            newTag.Name = Request.Form["Tag.Name"]; // New tag name (from text input)
            var selectedTag = Convert.ToInt64(Request.Form["RepoTag"]); // Selected from select (tag to be updated)
            newTag.Id = selectedTag;

            if (repoTagItem.Contains(_gitRepoTagService.GetByIdRepoTag(repoId, selectedTag)))
            {
                var result = _tagService.Update(newTag);

                if (!result.IsNullOrEmpty()) IsPostSuccess = true;
            }
        }
        
        public JsonResult OnGetPopulate(long id)
        {
            return new JsonResult(GetResponse(id));
        }

        public IEnumerable<SelectListItem> GetResponse(long id)
        {
            var query = (from rt in _gitRepoTagService.GetAll()
                join t in _tagService.GetAll() on rt.TagId equals t.Id
                where rt.GitRepoId == id
                select new SelectListItem
                {
                    Value = t.Id.ToString(),
                    Text = t.Name
                });
            return query.ToList();
        }
    }
}
