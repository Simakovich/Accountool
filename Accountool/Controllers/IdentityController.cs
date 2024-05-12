using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Accountool.Controllers
{
    public class IdentityController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;

        public IdentityController(UserManager<IdentityUser> userManager)
        {
            _userManager = userManager;
        }

        public IActionResult Index()
        {
            //using (var scope = _serviceScopeFactory.CreateScope())
            //{
                //var roleManager = scope.ServiceProvider.GetRequiredService<UserManager<IdentityRole>>();
                var users = _userManager.Users.ToList();
                return View("./Identity/Identities.cshtml", users);
                // Your code here
            }
        //}
        //private readonly UserManager<IdentityUser> _userManager;

        //public IdentityController(UserManager<IdentityUser> userManager)
        //{
        //    _userManager = userManager;
        //}
        //public IActionResult Index()
        //{
        //    var users = _userManager.Users.ToList();
        //    return View("./Identity/Identities.cshtml", users);
        //}
    }
}
