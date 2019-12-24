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
    public class FavouriteSongsController : Controller
    {
        private readonly MusicOnContext _context;

        public FavouriteSongsController(MusicOnContext context)
        {
            _context = context;
        }

        // GET: FavouriteSongs
        public async Task<IActionResult> Index()
        {
            var musicOnContext = _context.FavouriteSongs.Include(f => f.Composition).Include(f => f.User);
            return View(await musicOnContext.ToListAsync());
        }

        // GET: FavouriteSongs/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var favouriteSongs = await _context.FavouriteSongs
                .Include(f => f.Composition)
                .Include(f => f.User)
                .FirstOrDefaultAsync(m => m.UserId == id);
            if (favouriteSongs == null)
            {
                return NotFound();
            }

            return View(favouriteSongs);
        }

        // GET: FavouriteSongs/Create
        public IActionResult Create()
        {
            ViewData["CompositionId"] = new SelectList(_context.Composition, "CompositionId", "CompositionId");
            ViewData["UserId"] = new SelectList(_context.User, "UserId", "Email");
            return View();
        }

        // POST: FavouriteSongs/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("UserId,CompositionId,Date")] FavouriteSongs favouriteSongs)
        {
            if (ModelState.IsValid)
            {
                favouriteSongs.UserId = Guid.NewGuid();
                _context.Add(favouriteSongs);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CompositionId"] = new SelectList(_context.Composition, "CompositionId", "CompositionId", favouriteSongs.CompositionId);
            ViewData["UserId"] = new SelectList(_context.User, "UserId", "Email", favouriteSongs.UserId);
            return View(favouriteSongs);
        }

        // POST: FavouriteSongs/Add
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Add(Guid CompositionId)
        {
            if (ModelState.IsValid)
            {
                FavouriteSongs favouriteSong = new FavouriteSongs();
                favouriteSong.UserId = new Guid(User.FindFirst(x => x.Type == "id").Value);
                favouriteSong.CompositionId = CompositionId;
                _context.FavouriteSongs.Add(favouriteSong);
                await _context.SaveChangesAsync();
                return Redirect(Request.Headers["Referer"].ToString());
            }
            return Redirect(Request.Headers["Referer"].ToString());
        }

        // GET: FavouriteSongs/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var favouriteSongs = await _context.FavouriteSongs.FindAsync(id);
            if (favouriteSongs == null)
            {
                return NotFound();
            }
            ViewData["CompositionId"] = new SelectList(_context.Composition, "CompositionId", "CompositionId", favouriteSongs.CompositionId);
            ViewData["UserId"] = new SelectList(_context.User, "UserId", "Email", favouriteSongs.UserId);
            return View(favouriteSongs);
        }

        // POST: FavouriteSongs/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("UserId,CompositionId,Date")] FavouriteSongs favouriteSongs)
        {
            if (id != favouriteSongs.UserId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(favouriteSongs);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!FavouriteSongsExists(favouriteSongs.UserId))
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
            ViewData["CompositionId"] = new SelectList(_context.Composition, "CompositionId", "CompositionId", favouriteSongs.CompositionId);
            ViewData["UserId"] = new SelectList(_context.User, "UserId", "Email", favouriteSongs.UserId);
            return View(favouriteSongs);
        }

        // GET: FavouriteSongs/Delete/5
        public async Task<IActionResult> Delete(Guid? CompositionId)
        {
            if (CompositionId == null)
            {
                return NotFound();
            }

            var favouriteSongs = await _context.FavouriteSongs
                .Include(f => f.Composition)
                .Include(f => f.User)
                .Where(f => f.CompositionId == CompositionId)
                .FirstOrDefaultAsync(m => m.UserId == new Guid(User.FindFirst(x => x.Type == "id").Value));
            if (favouriteSongs == null)
            {
                return NotFound();
            }

            return View(favouriteSongs);
        }

        // POST: FavouriteSongs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid CompositionId)
        {
            var favouriteSongs = await _context.FavouriteSongs
                .Where(f => f.CompositionId == CompositionId)
                .FirstOrDefaultAsync(m => m.UserId == new Guid(User.FindFirst(x => x.Type == "id").Value));

            _context.FavouriteSongs.Remove(favouriteSongs);
            await _context.SaveChangesAsync();

            return Redirect(Request.Headers["Referer"].ToString());
        }

        private bool FavouriteSongsExists(Guid id)
        {
            return _context.FavouriteSongs.Any(e => e.UserId == id);
        }
    }
}
