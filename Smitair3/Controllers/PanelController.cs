using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Smitair3.Data;
using Smitair3.Models;
using SmitairDOTNET.DAL;
using SmitairDOTNET.Models;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace SmitairDOTNET.Controllers
{
    public class PanelController : Controller
    {
        private readonly SmitairDbContext _context;
        private readonly IHostingEnvironment _hosting;
        public static ApplicationUser dataUser;
        public static Effect effects;
        public static Purchase purchases;
        public static bool purchased;

        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;

        private ApplicationDbContext _appdbcontext;

        public PanelController(SmitairDbContext context, IHostingEnvironment hosting,
            ApplicationDbContext appdbcontext, UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
        {
            _context = context;
            _hosting = hosting;
            _userManager = userManager;
            _signInManager = signInManager;
            _appdbcontext = appdbcontext;
        }

        private void layout()
        {
            //ViewBag.FirstName = dataUser.FirstName;
            //ViewBag.EmailAdress = dataUser.Email;
            //ViewBag.AvatarLink = dataUser.AvatarLink;
            //ViewBag.LastName = dataUser.LastName;
        }

        // GET: Panel
        public ActionResult Index()
        {
            return RedirectToAction("LogAgain");
        }

        // Log Again
        public ActionResult LogAgain()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Index(ApplicationUser objUser)
        {
            bool findedUser = false;
            //public var dataUser = objUser;
            foreach (ApplicationUser user in _userManager.Users)
            {
                if (user.UserName == objUser.UserName && user.PasswordHash == objUser.PasswordHash)
                {
                    findedUser = true;
                    dataUser = user;
                    break;
                }
            }

            layout();

            if (findedUser)
            {
                return View(dataUser);
            }
            else
            {
                return RedirectToAction("LogIn");
            }
        }

        //LogIn
        public ActionResult LogIn()
        {
            return View();
        }

        //MyAccount
        public ActionResult MyAccount()
        {
            layout();

            ViewData["User"] = dataUser.UserName;
            ViewData["FirstName"] = dataUser.FirstName;
            ViewData["LastName"] = dataUser.LastName;
            ViewData["EmailAdress"] = dataUser.Email;

            return View(dataUser);
        }

        // GET: Panel/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Panel/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Panel/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: Panel/Edit/5
        [HttpGet]
        //[ValidateAntiForgeryToken]
        public ActionResult Edit()
        {
            layout();

            return View(dataUser);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind("ID,User,Password,FirstName,LastName,AvatarLink,EmailAdress")] ApplicationUser users)
        {
            //change editing profile foto

            dataUser.FirstName = users.FirstName;
            dataUser.LastName = users.LastName;
            dataUser.Email = users.Email;
            _context.Update(dataUser);
            _context.SaveChanges();
            return RedirectToAction("MyAccount");
        }

        public ActionResult AddEffect()
        {
            layout();

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AddEffect([Bind("EffectName,YoutubeLink,Description")] Effect effect, IFormFile file)
        {
            if (ModelState.IsValid)
            {
                var uploads = Path.Combine(_hosting.WebRootPath, "hosting\\Effects\\" + effect.EffectID + ".smi");
                if (file != null && file.Length > 0)
                {
                    using (FileStream fs = System.IO.File.Create(uploads))
                    {
                        file.CopyTo(fs);
                        fs.Flush();
                    }
                    effect.EffectLink = "/hosting/Effects/" + effect.EffectID + ".smi";
                }

                effect.YoutubeLink = effect.YoutubeLink.Replace("https://www.youtube.com/watch?v=", "https://www.youtube.com/embed/") + "?ecver=2";

                effect.User = _userManager.Users.Where(user => user.Id == dataUser.Id).Single();

                _context.Effects.Add(effect);

                _context.SaveChanges();

                return RedirectToAction("MyAccount");
            }
            return View(dataUser);
        }

        public ActionResult Library()
        {
            layout();

            var myPurchases = (from x in _context.Purchases
                               where x.User == dataUser
                               select x).Include(y => y.Effect).Include(z => z.Effect.User).ToList();

            return View(/*_context.Effects.ToList()*/myPurchases);
        }

        [HttpPost]
        public ActionResult Library(int? id, int? grade, Purchase purchase, FormCollection form)
        {
            if(id == null)
            {
                return RedirectToAction("MyAccount");
            }
            else
            {
                purchase = _context.Purchases.Where(purch => purch.ID == id).Single();
                

                _context.Update(purchase);
                _context.SaveChanges();

                return RedirectToAction("MyAccount");
            }
        }

        public ActionResult Shop(int? idd, Purchase purchases)
        {
            if (idd == null)
            {
                layout();
                if (purchased) ViewBag.Purchase = "You have bought effect!";
                purchased = false;

                var effects = _context.Effects.Include(x => x.User).ToList();

                return View(effects);
            }
            else
            {
                var purchase = _context.Effects.SingleOrDefault(m => m.EffectID == idd);

                //purchases.EffectID = purchase.EffectID;
                //purchases.UserID = dataUser.ID;
                purchases.Effect = _context.Effects.Where(effect => effect.EffectID == idd).Single();
                purchases.User = _userManager.Users.Where(user => user.Id == dataUser.Id).Single();

                _context.Add(purchases);
                _context.SaveChanges();
                purchased = true;

                if (purchase == null)
                {
                    return NotFound();
                }
            }

            return RedirectToAction("Shop");
        }

        // GET: Panel/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Panel/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}