using BeautifyMe.Areas.Identity.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace BeautifyMe.Controllers
{
    public class SessionController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        public SessionController(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        [HttpGet]
        public async Task<string> GetUserSessionInfo()
        {
            var user =await _userManager.FindByEmailAsync(User.Identity.Name);            
            return user.SessionId.ToString();
        }
    }
}
