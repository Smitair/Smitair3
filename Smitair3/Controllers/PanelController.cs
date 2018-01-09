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

        FileStore _store = new FileStore();

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

            if (_userManager.GetUserId(User) != null)
            {
                var user = _context.Users.Where(us => us.Id == _userManager.GetUserId(User)).Single();

                if (user.AvatarLink != null)
                {
                    string avatarcurrent = _context.Users.Where(us => us.Id == user.Id).Single().AvatarLink;
                    user.AvatarCurrent = _store.UriForImage(avatarcurrent).ToString();
                }
            }
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
                var user = await _userManager.GetUserAsync(User);

                _context.Effects.Add(effect);
                await _context.SaveChangesAsync();

                if (file != null && file.Length > 0)
                {
                    var id = effect.EffectID.ToString();
                    var effectId = await _store.SaveEffect(file.OpenReadStream(), id);
                    effect.EffectLink = effectId;
                }

                effect.YoutubeLink = effect.YoutubeLink.Replace("https://www.youtube.com/watch?v=",
                    "https://www.youtube.com/embed/") + "?ecver=2";

                effect.User = _context.Users.Where(us => us.Id == user.Id).Single();
                _context.Effects.Update(effect);
                await _context.SaveChangesAsync();

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

        public async Task<ActionResult> Shop(Guid? idd, Purchase purchase)
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
                purchase.User = _context.Users.Where(us => us.Id == user.Id).Single();
                purchase.Effect = _context.Effects.Where(effect => effect.EffectID == idd).Single();

                string effectCurrent = _context.Effects.Where(ef => ef.EffectID == idd).Single().EffectID.ToString();
                purchase.Download = _store.UriForEffect(effectCurrent).ToString();

                _context.Purchases.Add(purchase);
                await _context.SaveChangesAsync();

                purchased = true;
            }

            return RedirectToAction("Shop");
        }
    }
}