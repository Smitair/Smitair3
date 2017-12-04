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

namespace SmitairDOTNET.Controllers
{
    public class SignUpController : Controller
    {
        private readonly SmitairDbContext _context;
        private readonly IHostingEnvironment _hosting;

        public SignUpController(SmitairDbContext context, IHostingEnvironment hosting)
        {
            _context = context;
            _hosting = hosting;
        }

        // GET: SignUp
        public async Task<IActionResult> Index()
        {
            return View(await _context.Users.ToListAsync());
        }

        // GET: SignUp/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var users = await _context.Users
                .SingleOrDefaultAsync(m => m.ID == id);
            if (users == null)
            {
                return NotFound();
            }

            return View(users);
        }

        // GET: SignUp/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: SignUp/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,Username,Password,ConfirmPassword," +
            "FirstName,LastName,EmailAdress,ConfirmEmailAdress")] User users, IFormFile file)
        {
            if (ModelState.IsValid)
            {
                var isAnyNick = _context.Users.FirstOrDefault(us => us.Username == users.Username);
                var isAnyMail = _context.Users.FirstOrDefault(us => us.EmailAdress == users.EmailAdress);

                if (isAnyNick == null || isAnyMail == null)
                {
                    var uploads = Path.Combine(_hosting.WebRootPath, "images\\UserAvatar\\" + users.Username + ".jpg");
                    if (file != null && file.Length > 0)
                    {
                        using (FileStream fs = System.IO.File.Create(uploads))
                        {
                            file.CopyTo(fs);
                            fs.Flush();
                        }
                        users.AvatarLink = "/images/UserAvatar/" + users.Username + ".jpg";
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

        // GET: SignUp/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var users = await _context.Users.SingleOrDefaultAsync(m => m.ID == id);
            if (users == null)
            {
                return NotFound();
            }
            return View(users);
        }

        // POST: SignUp/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,User,Password,FirstName,LastName,AvatarLink")] User users)
        {
            if (id != users.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(users);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UsersExists(users.ID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(users);
        }

        // GET: SignUp/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var users = await _context.Users
                .SingleOrDefaultAsync(m => m.ID == id);
            if (users == null)
            {
                return NotFound();
            }

            return View(users);
        }

        // POST: SignUp/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var users = await _context.Users.SingleOrDefaultAsync(m => m.ID == id);
            _context.Users.Remove(users);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool UsersExists(int id)
        {
            return _context.Users.Any(e => e.ID == id);
        }
    }
}