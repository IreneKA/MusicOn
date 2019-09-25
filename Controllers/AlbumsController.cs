using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MusicOn.Logic;
using MusicOn.Models;
using MusicOn.ViewModels;

namespace MusicOn.Controllers
{
    public class AlbumsController : Controller
    {
        private readonly MusicOnContext _context;

        public AlbumsController(MusicOnContext context)
        {
            _context = context;
        }

        // GET: Albums
        public async Task<IActionResult> Index()
        {
            var musicOnContext = _context.Album.Include(a => a.Genre).Include(a => a.AlbumsArtists);
            return View(await musicOnContext.ToListAsync());
        }

        // GET: Albums/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var album = await _context.Album
                .Include(a => a.Genre)
                .Include(a => a.AlbumsArtists)
                .ThenInclude(a=>a.Artist)
                .Include(a => a.Composition)
                    .ThenInclude(c => c.Song)
                .FirstOrDefaultAsync(m => m.AlbumId == id);
            if (album == null)
            {
                return NotFound();
            }
            AlbumViewModel albumView = new AlbumViewModel();
            if (User.Identity.IsAuthenticated)
            {
                var currentUserId = new Guid(User.FindFirst(x => x.Type == "id").Value);
                albumView.Compositions = ViewModelConstructor.FavouriteSongs(album.Composition.ToList(), _context, currentUserId);
            }
            else
            {
                foreach (var comp in album.Composition.ToList())
                {
                    var compVM = new CompositionViewModel() { Composition = comp };
                    albumView.Compositions.Add(compVM);
                }
            }
            albumView.Album = album;
            

            return View(albumView);
        }

        // GET: Albums/Create
        public IActionResult Create()
        {
            ViewData["GenreId"] = new SelectList(_context.Genre, "GenreId", "Title");
            return View();
        }

        // POST: Albums/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("AlbumId,Title,Date,Single,GenreId,GroupeId")] Album album)
        {
            if (ModelState.IsValid)
            {
                album.AlbumId = Guid.NewGuid();
                _context.Add(album);
                await _context.SaveChangesAsync();
                return RedirectToAction("Details", new { id = album.AlbumId });
            }
            ViewData["GenreId"] = new SelectList(_context.Genre, "GenreId", "Title", album.GenreId);
            return View(album);
        }

        // GET: Albums/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var album = await _context.Album.FindAsync(id);
            if (album == null)
            {
                return NotFound();
            }

            ViewData["GenreId"] = new SelectList(_context.Genre, "GenreId", "Title", album.GenreId);
            return View(album);
        }

        // POST: Albums/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("AlbumId,Title,Date,Single,GenreId,GroupeId")] Album album)
        {
            if (id != album.AlbumId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(album);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AlbumExists(album.AlbumId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("Details", new { id = album.AlbumId });
            }

            ViewData["GenreId"] = new SelectList(_context.Genre, "GenreId", "Title", album.GenreId);
            return View(album);
        }

        // GET: Albums/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var album = await _context.Album
                .Include(a => a.Genre)
                .Include(a => a.AlbumsArtists)
                .FirstOrDefaultAsync(m => m.AlbumId == id);
            if (album == null)
            {
                return NotFound();
            }

            return View(album);
        }

        // POST: Albums/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var album = await _context.Album.FindAsync(id);
            _context.Album.Remove(album);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index","Search");
        }

        private bool AlbumExists(Guid id)
        {
            return _context.Album.Any(e => e.AlbumId == id);
        }
    }
}
