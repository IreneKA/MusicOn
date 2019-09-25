using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MusicOn.Models;

namespace MusicOn.Controllers
{
    public class FavouriteArtistsController : Controller
    {
        private readonly MusicOnContext _context;

        public FavouriteArtistsController(MusicOnContext context)
        {
            _context = context;
        }

        // GET: FavouriteArtists
        public async Task<IActionResult> Index()
        {
            var musicOnContext = _context.FavouriteArtists.Include(f => f.Artist).Include(f => f.User);
            return View(await musicOnContext.ToListAsync());
        }

        // GET: FavouriteArtists/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var favouriteArtists = await _context.FavouriteArtists
                .Include(f => f.Artist)
                .Include(f => f.User)
                .FirstOrDefaultAsync(m => m.UserId == id);
            if (favouriteArtists == null)
            {
                return NotFound();
            }

            return View(favouriteArtists);
        }

        // GET: FavouriteArtists/Create
        public IActionResult Create()
        {
            ViewData["ArtistId"] = new SelectList(_context.Artist, "ArtistId", "Name");
            ViewData["UserId"] = new SelectList(_context.User, "UserId", "Email");
            return View();
        }

        // POST: FavouriteArtists/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("UserId,ArtistId,Date")] FavouriteArtists favouriteArtists)
        {
            if (ModelState.IsValid)
            {
                favouriteArtists.UserId = Guid.NewGuid();
                _context.Add(favouriteArtists);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ArtistId"] = new SelectList(_context.Artist, "ArtistId", "Name", favouriteArtists.ArtistId);
            ViewData["UserId"] = new SelectList(_context.User, "UserId", "Email", favouriteArtists.UserId);
            return View(favouriteArtists);
        }

        // GET: FavouriteArtists/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var favouriteArtists = await _context.FavouriteArtists.FindAsync(id);
            if (favouriteArtists == null)
            {
                return NotFound();
            }
            ViewData["ArtistId"] = new SelectList(_context.Artist, "ArtistId", "Name", favouriteArtists.ArtistId);
            ViewData["UserId"] = new SelectList(_context.User, "UserId", "Email", favouriteArtists.UserId);
            return View(favouriteArtists);
        }

        // POST: FavouriteArtists/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("UserId,ArtistId,Date")] FavouriteArtists favouriteArtists)
        {
            if (id != favouriteArtists.UserId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(favouriteArtists);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!FavouriteArtistsExists(favouriteArtists.UserId))
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
            ViewData["ArtistId"] = new SelectList(_context.Artist, "ArtistId", "Name", favouriteArtists.ArtistId);
            ViewData["UserId"] = new SelectList(_context.User, "UserId", "Email", favouriteArtists.UserId);
            return View(favouriteArtists);
        }

        // GET: FavouriteArtists/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var favouriteArtists = await _context.FavouriteArtists
                .Include(f => f.Artist)
                .Include(f => f.User)
                .FirstOrDefaultAsync(m => m.UserId == id);
            if (favouriteArtists == null)
            {
                return NotFound();
            }

            return View(favouriteArtists);
        }

        // POST: FavouriteArtists/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid ArtistId)
        {
            var favouriteArtists = await _context.FavouriteArtists
                .Where(f => f.ArtistId == ArtistId)
                .FirstOrDefaultAsync(m => m.UserId == new Guid(User.FindFirst(x => x.Type == "id").Value));
            _context.FavouriteArtists.Remove(favouriteArtists);
            await _context.SaveChangesAsync();
            return Redirect(Request.Headers["Referer"].ToString());
        }

        private bool FavouriteArtistsExists(Guid id)
        {
            return _context.FavouriteArtists.Any(e => e.UserId == id);
        }
        // POST: FavouriteSongs/Add
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Add(Guid ArtistId)
        {
            if (ModelState.IsValid)
            {
                FavouriteArtists favouriteArtist = new FavouriteArtists();
                favouriteArtist.UserId = new Guid(User.FindFirst(x => x.Type == "id").Value);
                favouriteArtist.ArtistId = ArtistId;
                _context.FavouriteArtists.Add(favouriteArtist);
                await _context.SaveChangesAsync();
                return Redirect(Request.Headers["Referer"].ToString());
            }
            return Redirect(Request.Headers["Referer"].ToString());
        }
    }
}
