using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;

namespace GitTagApp.Controllers
{
    [Route("[controller]/[action]")]
    public class LoginController : Controller
    {
        // GET
        [HttpGet]
        public IActionResult Login(string returnUrl = "/Main")
        {
            return Challenge(new AuthenticationProperties {RedirectUri = returnUrl});
        }
    }
}