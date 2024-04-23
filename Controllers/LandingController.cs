using BeautifyMe.Areas.Identity.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace BeautifyMe.Controllers
{
    public class LandingController : Controller
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        public LandingController(SignInManager<ApplicationUser> signInManager)
        {
                _signInManager = signInManager;
        }
        public IActionResult Index()
        {
            if(_signInManager.IsSignedIn(User))
            {
                return RedirectToAction("Index", "Home");
            }
            return View();
        }
    }
}
