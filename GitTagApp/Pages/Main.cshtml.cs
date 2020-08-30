using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

namespace GitTagApp.Pages
{
    public class MainModel : PageModel
    {
        public string RequestId { get; set; }

        private readonly ILogger<ErrorModel> _logger;

        public MainModel(ILogger<ErrorModel> logger)
        {
            _logger = logger;
        }

        public void OnGet()
        {
        }
    }
}
