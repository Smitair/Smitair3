using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Smitair3.Data;
using Smitair3.Models;
using System.Diagnostics;
using System.Linq;

namespace SmitairDOTNET.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        public static bool loggedIn;

        FileStore _store = new FileStore();

        public HomeController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if(_userManager.GetUserId(User) != null)
            {
                var user = _context.Users.Where(us => us.Id == _userManager.GetUserId(User)).Single();

                if (user.AvatarLink != null)
                {
                    string avatarcurrent = _context.Users.Where(us => us.Id == user.Id).Single().AvatarLink;
                    user.AvatarCurrent = _store.UriForImage(avatarcurrent).ToString();
                }
            }
        }

        public IActionResult Index()
        {
            ViewBag.MainPage = 1;
            return View();
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public IActionResult signinfacebook()
        {
            return View();
        }
    }
}