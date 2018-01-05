using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore;
using Smitair3.Data;
using Smitair3.Models;
using SmitairDOTNET.Models;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace SmitairDOTNET.Controllers
{
    [Authorize]
    public class PanelController : Controller
    {
        private readonly IHostingEnvironment _hosting;
        public static Effect effects;
        public static Purchase purchases;
        public static bool purchased;

        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private ApplicationDbContext _context;

        public PanelController(ApplicationDbContext context, IHostingEnvironment hosting,
            UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
        {
            _context = context;
            _hosting = hosting;
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            ViewBag.Panel = 1;
        }

        public ActionResult AddEffect()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddEffect([Bind("EffectName,YoutubeLink,Description")]
        Effect effect, Purchase purchase ,IFormFile file)
        {
            if (ModelState.IsValid)
            {
                var user = _userManager.GetUserId(User);

                var uploads = Path.Combine(_hosting.WebRootPath,
                    "hosting\\Effects\\" + effect.EffectID + ".smi");
                if (file != null && file.Length > 0)
                {
                    using (FileStream fs = System.IO.File.Create(uploads))
                    {
                        file.CopyTo(fs);
                        fs.Flush();
                    }
                    effect.EffectLink = "/hosting/Effects/" + effect.EffectID + ".smi";
                }

                effect.YoutubeLink = effect.YoutubeLink.Replace("https://www.youtube.com/watch?v=",
                    "https://www.youtube.com/embed/") + "?ecver=2";

                var loggedUser = _context.Users.Where(us => us.Id == user).Single();
                effect.User = loggedUser;
                _context.Effects.Add(effect);
                _context.SaveChanges();

                return RedirectToAction("AddEffect");
            }
            return View();
        }

        public async Task<ActionResult> Library()
        {
            var user = await _userManager.GetUserAsync(User);

            var myPurchases = (from x in _context.Purchases
                               where x.User == user
                               select x).Include(y => y.Effect).Include(z => z.Effect.User).ToList();

            return View(myPurchases);
        }

        [HttpPost]
        public async Task<ActionResult> Library(Guid? id, int? grade, Purchase purchase, FormCollection form)
        {
            var user = await _userManager.GetUserAsync(User);

            if (id == null)
            {
                return RedirectToAction("Index", "Manage");
            }
            else
            {
                purchase = _context.Purchases.Where(purch => purch.PurchaseID == id).Single();

                _context.Update(purchase);
                _context.SaveChanges();

                return RedirectToAction("Index", "Manage");
            }
        }

        public async Task<ActionResult> Shop(Guid? idd, Purchase purchases)
        {
            var user = await _userManager.GetUserAsync(User);

            if (idd == null)
            {
                if (purchased) ViewBag.Purchase = "You have bought effect!";
                purchased = false;

                var effects = _context.Effects.Include(x => x.User).ToList();

                return View(effects);
            }
            else
            {
                _context.Purchases.Add(purchases);
                _context.SaveChanges();

                //purchases.User = user;
                purchases.User = _userManager.Users.Where(us => us.Id == user.Id).Single();
                purchases.Effect = _context.Effects.Where(effect => effect.EffectID == idd).Single();

                _context.Purchases.Update(purchases);
                _context.SaveChanges();

                purchased = true;
            }

            return RedirectToAction("Shop");
        }
    }
}