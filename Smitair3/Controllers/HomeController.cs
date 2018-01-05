using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Smitair3.Data;
using Smitair3.Models;
using System.Diagnostics;

namespace SmitairDOTNET.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        public static bool loggedIn;

        public HomeController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
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