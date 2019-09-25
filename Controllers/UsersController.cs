using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MusicOn.Logic;
using MusicOn.Models;
using MusicOn.ViewModels;

namespace MusicOn.Controllers
{
    public class UsersController : Controller
    {
        private readonly MusicOnContext _context;

        public UsersController(MusicOnContext context)
        {
            _context = context;
        }

        // GET: Users
        public async Task<IActionResult> Index(string searchString)
        {
            var users = from c in _context.User
                        .Include(u => u.Role)
                        select c;
            if (!String.IsNullOrEmpty(searchString))
            {
                users = users.Where(s => s.Username.Contains(searchString));
            }
            return View(await users.ToListAsync());
        }
        // GET: Users/Details/5
        [Authorize]
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound("id == null");
            }

            var user = await _context.User
                .Include(u => u.Role)
                .Include(u=>u.FavouriteSongs)
                    .ThenInclude(fs=>fs.Composition.Album.AlbumsArtists)
                        .ThenInclude(ga=>ga.Artist)
                .Include(u => u.FavouriteSongs)
                    .ThenInclude(fs => fs.Composition.Song)
                .Include(u=>u.FavouriteArtists)
                    .ThenInclude(fa=>fa.Artist)
                        .ThenInclude(a=>a.AlbumsArtists)
                            .ThenInclude(g=>g.Album)
                .Include(u=>u.LoadedSongs)
                    .ThenInclude(ls => ls.Composition.Song)
                .FirstOrDefaultAsync(u => u.UserId == id);
            if (user == null)
            {
                return NotFound("user == null");
            }
            var userVM = new UserViewModel() { User = user};
            var compositionsF = new List<Composition>();
            foreach (var song in user.FavouriteSongs)
            {
                compositionsF.Add(song.Composition);
            }
            Guid currentUserId = new Guid(User.FindFirst(x => x.Type == "id").Value);
            userVM.FavouriteCompositions = ViewModelConstructor.FavouriteSongs(compositionsF, _context, currentUserId);
            var artistsF = new List<Artist>();
            foreach (var artist in user.FavouriteArtists)
            {
                artistsF.Add(artist.Artist);
            }
            userVM.FavouriteArtists = ViewModelConstructor.FavouriteArtists(artistsF, _context, currentUserId);
            userVM.LoadedSongs = user.LoadedSongs.ToList();
            return View(userVM);
        }

        // GET: Users/Create
        public IActionResult Create()
        {
            ViewData["RoleId"] = new SelectList(_context.Role, "RoleId", "Title");
            return View();
        }

        // POST: Users/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("UserId,Username,Email,Password,RoleId")] User user)
        {
            if (ModelState.IsValid)
            {
                user.UserId = Guid.NewGuid();
                _context.Add(user);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["RoleId"] = new SelectList(_context.Role, "RoleId", "Title", user.RoleId);
            return View(user);
        }

        // GET: Users/Edit/5
        public async Task<IActionResult> EditRole(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _context.User.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            ViewData["RoleId"] = new SelectList(_context.Role, "RoleId", "Title", user.RoleId);
            return View(user);
        }

        // POST: Users/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditRole(Guid id, [Bind("UserId,Username,Email,Password,RoleId")] User user)
        {
            if (id != user.UserId)
            {
                return NotFound("id != user.UserId");
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(user);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UserExists(user.UserId))
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
            ViewData["RoleId"] = new SelectList(_context.Role, "RoleId", "Title", user.RoleId);
            return View(user);
        }

        // GET: Users/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _context.User
                .Include(u => u.Role)
                .FirstOrDefaultAsync(m => m.UserId == id);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        // POST: Users/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var user = await _context.User.FindAsync(id);
            _context.User.Remove(user);
            await _context.SaveChangesAsync();
            if(id.ToString() == User.FindFirst(x=>x.Type == "id").Value) return RedirectToAction("Logout","Account");
            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _context.User.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            return View(user);
        }

        // POST: Users/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("UserId,Username,Email,Password,RoleId")] User user)
        {
            if (id != user.UserId)
            {
                return NotFound("id != user.UserId");
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(user);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UserExists(user.UserId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                Role role = await _context.Role.FirstOrDefaultAsync(r => r.RoleId == user.RoleId);
                user.Role = role;
                var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.Email),
                new Claim(ClaimTypes.Role, user.Role.Title),
                new Claim("name", user.Username),
                new Claim("id",user.UserId.ToString())
            };
                // создаем объект ClaimsIdentity
                ClaimsIdentity cid = new ClaimsIdentity(claims, "ApplicationCookie", ClaimTypes.Name, ClaimTypes.Role);
                await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(cid));

                return RedirectToAction(nameof(Details),new { id=user.UserId});
            }
            return View(user);
        }

        private bool UserExists(Guid id)
        {
            return _context.User.Any(e => e.UserId == id);
        }
    }
}
