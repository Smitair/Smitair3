using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SmitairDOTNET.Models;
using SmitairDOTNET.DAL;
using Smitair3.Models;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Identity;

namespace SmitairDOTNET.Controllers
{
    public class HomeController : Controller
    {
        private readonly SmitairDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        public static bool loggedIn;

        public HomeController(SmitairDbContext context, UserManager<ApplicationUser> userManager)
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
