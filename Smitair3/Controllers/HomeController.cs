using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SmitairDOTNET.Models;
using SmitairDOTNET.DAL;
using Smitair3.Models;

namespace SmitairDOTNET.Controllers
{
    public class HomeController : Controller
    {
        private readonly SmitairDbContext _context;

        public HomeController(SmitairDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
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
