using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SmitairDOTNET.DAL;
using SmitairDOTNET.Models;
using System.Data.SqlClient;
using Microsoft.AspNetCore.Mvc;
using System.Collections;
using System.IO;
using System.Web;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Hosting;
using System.Net.Http.Headers;
using Smitair3.Data;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Smitair3.Models;
using Microsoft.AspNetCore.DataProtection.KeyManagement;
using Microsoft.AspNetCore.Identity;
using Smitair3.Services;
using Microsoft.Extensions.Logging;
using System.Text.Encodings.Web;

namespace SmitairDOTNET.Controllers
{
    public class SignUpController : Controller
    {
        private readonly SmitairDbContext _context;
        private readonly IHostingEnvironment _hosting;
        public static ApplicationUser dataUser;

        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;

        private ApplicationDbContext _appdbcontext;

        public SignUpController(SmitairDbContext context, IHostingEnvironment hosting,
            ApplicationDbContext appdbcontext, UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
        {
            _context = context;
            _hosting = hosting;
            _userManager = userManager;
            _signInManager = signInManager;
            _appdbcontext = appdbcontext;
        }

        // GET: SignUp
        public async Task<IActionResult> Index()
        {
            return View(await _userManager.Users.ToListAsync());
        }

        // GET: SignUp/Details/5
        //public async Task<IActionResult> Details(int? id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        //    var users = await _userManager.Users
        //        .SingleOrDefaultAsync(m => m.Id == id);
        //    if (users == null)
        //    {
        //        return NotFound();
        //    }

        //    return View(users);
        //}

        // GET: SignUp/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: SignUp/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,Username,Password,ConfirmPassword," +
            "FirstName,LastName,EmailAdress,ConfirmEmailAdress")] ApplicationUser users, IFormFile file)
        {
            if (ModelState.IsValid)
            {
                var isAnyNick = _userManager.Users.FirstOrDefault(us => us.UserName == users.UserName);
                var isAnyMail = _userManager.Users.FirstOrDefault(us => us.Email == users.Email);

                if (isAnyNick == null || isAnyMail == null)
                {
                    var uploads = Path.Combine(_hosting.WebRootPath, "images\\UserAvatar\\" + users.UserName + ".jpg");
                    if (file != null && file.Length > 0)
                    {
                        using (FileStream fs = System.IO.File.Create(uploads))
                        {
                            file.CopyTo(fs);
                            fs.Flush();
                        }
                        users.AvatarLink = "/images/UserAvatar/" + users.UserName + ".jpg";
                    }

                    _context.Add(users);
                    await _context.SaveChangesAsync();

                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    if (isAnyNick != null) ModelState.AddModelError("User", "This username already exist. Chose another one");
                    if (isAnyMail != null) ModelState.AddModelError("EmailAdress", "Somebody has this account in this service. Choose another");
                }
            }
            return View(users);
        }

        // GET: SignUp/Edit/5 !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
        //public async Task<IActionResult> Edit(int? id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        //    var users = await _userManager.Users.SingleOrDefaultAsync(m => m.Id == id);
        //    if (users == null)
        //    {
        //        return NotFound();
        //    }
        //    return View(users);
        //}

        // POST: SignUp/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Edit(int id, [Bind("ID,User,Password,FirstName,LastName,AvatarLink")] ApplicationUser users)
        //{
        //    if (id != users.Id)
        //    {
        //        return NotFound();
        //    }

        //    if (ModelState.IsValid)
        //    {
        //        try
        //        {
        //            _context.Update(users);
        //            await _context.SaveChangesAsync();
        //        }
        //        catch (DbUpdateConcurrencyException)
        //        {
        //            if (!UsersExists(users.ID))
        //            {
        //                return NotFound();
        //            }
        //            else
        //            {
        //                throw;
        //            }
        //        }
        //        return RedirectToAction(nameof(Index));
        //    }
        //    return View(users);
        //}

        // GET: SignUp/Delete/5
        //public async Task<IActionResult> Delete(int? id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        //    var users = await _userManager.Users
        //        .SingleOrDefaultAsync(m => m.Id == id);
        //    if (users == null)
        //    {
        //        return NotFound();
        //    }

        //    return View(users);
        //}

        // POST: SignUp/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> DeleteConfirmed(int id)
        //{
        //    var users = await _userManager.Users.SingleOrDefaultAsync(m => m.Id == id);
        //    _userManager.Users.Remove(users);
        //    await _context.SaveChangesAsync();
        //    return RedirectToAction(nameof(Index));
        //}

        //private bool UsersExists(int id)
        //{
        //    return _userManager.Users.Any(e => e.Id == id);
        //}
    }
}