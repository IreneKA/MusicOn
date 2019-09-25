using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MusicOn.Models;

namespace MusicOn.Controllers
{
    public class LoadedSongsController : Controller
    {
        private readonly MusicOnContext _context;

        public LoadedSongsController(MusicOnContext context)
        {
            _context = context;
        }

        // POST: LoadedSongs/DeleteAll/
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteAll()
        {
            var currentUserId = new Guid(User.FindFirst(x => x.Type == "id").Value);

            var loadedSongs = await _context.LoadedSongs
                .Where(ls => ls.UserId == currentUserId)
                .ToListAsync();
            foreach (var loadedSong in loadedSongs)
            {
                _context.LoadedSongs.Remove(loadedSong);
            }
            await _context.SaveChangesAsync();

            return Redirect(Request.Headers["Referer"].ToString());
        }

        // GET: LoadedSongs
        public async Task<IActionResult> Index()
        {
            var musicOnContext = _context.LoadedSongs
                .Include(l => l.Composition)
                .Include(l => l.User);
            return View(await musicOnContext.ToListAsync());
        }

        // GET: LoadedSongs/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var loadedSongs = await _context.LoadedSongs
                .Include(l => l.Composition)
                .Include(l => l.User)
                .FirstOrDefaultAsync(m => m.UserId == id);
            if (loadedSongs == null)
            {
                return NotFound();
            }

            return View(loadedSongs);
        }

        // GET: LoadedSongs/Create
        public IActionResult Create()
        {
            ViewData["CompositionId"] = new SelectList(_context.Composition, "CompositionId", "CompositionId");
            ViewData["UserId"] = new SelectList(_context.User, "UserId", "Email");
            return View();
        }

        // POST: LoadedSongs/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("UserId,CompositionId,Date")] LoadedSongs loadedSongs)
        {
            if (ModelState.IsValid)
            {
                loadedSongs.UserId = Guid.NewGuid();
                _context.Add(loadedSongs);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CompositionId"] = new SelectList(_context.Composition, "CompositionId", "CompositionId", loadedSongs.CompositionId);
            ViewData["UserId"] = new SelectList(_context.User, "UserId", "Email", loadedSongs.UserId);
            return View(loadedSongs);
        }

        // GET: LoadedSongs/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var loadedSongs = await _context.LoadedSongs.FindAsync(id);
            if (loadedSongs == null)
            {
                return NotFound();
            }
            ViewData["CompositionId"] = new SelectList(_context.Composition, "CompositionId", "CompositionId", loadedSongs.CompositionId);
            ViewData["UserId"] = new SelectList(_context.User, "UserId", "Email", loadedSongs.UserId);
            return View(loadedSongs);
        }

        // POST: LoadedSongs/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("UserId,CompositionId,Date")] LoadedSongs loadedSongs)
        {
            if (id != loadedSongs.UserId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(loadedSongs);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!LoadedSongsExists(loadedSongs.UserId))
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
            ViewData["CompositionId"] = new SelectList(_context.Composition, "CompositionId", "CompositionId", loadedSongs.CompositionId);
            ViewData["UserId"] = new SelectList(_context.User, "UserId", "Email", loadedSongs.UserId);
            return View(loadedSongs);
        }

        // GET: LoadedSongs/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var loadedSongs = await _context.LoadedSongs
                .Include(l => l.Composition)
                .Include(l => l.User)
                .FirstOrDefaultAsync(m => m.UserId == id);
            if (loadedSongs == null)
            {
                return NotFound();
            }

            return View(loadedSongs);
        }

        // POST: LoadedSongs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid CompositionId)
        {
            var loadedSongs = await _context.LoadedSongs
                .Where(f => f.CompositionId == CompositionId)
                .FirstOrDefaultAsync(m => m.UserId == new Guid(User.FindFirst(x => x.Type == "id").Value));

            _context.LoadedSongs.Remove(loadedSongs);
            await _context.SaveChangesAsync();

            return Redirect(Request.Headers["Referer"].ToString());
        }

        private bool LoadedSongsExists(Guid id)
        {
            return _context.LoadedSongs.Any(e => e.UserId == id);
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
                LoadedSongs loadedSong = new LoadedSongs();
                loadedSong.UserId = new Guid(User.FindFirst(x => x.Type == "id").Value);
                loadedSong.CompositionId = CompositionId;
                loadedSong.Date = DateTime.Now;
                _context.LoadedSongs.Add(loadedSong);
                await _context.SaveChangesAsync();
                return Redirect(Request.Headers["Referer"].ToString());
            }
            return Redirect(Request.Headers["Referer"].ToString());
        }
    }
}
