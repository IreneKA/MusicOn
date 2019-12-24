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
    public class ArtistsController : Controller
    {
        private readonly MusicOnContext _context;

        public ArtistsController(MusicOnContext context)
        {
            _context = context;
        }

        // GET: Artists
        public async Task<IActionResult> Index()
        {
            return View(await _context.Artist.ToListAsync());
        }

        // GET: Artists/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var artist = await _context.Artist
                .Include(a => a.AlbumsArtists)
                    .ThenInclude(a => a.Album)
                         .ThenInclude(a => a.Composition)
                .FirstOrDefaultAsync(m => m.ArtistId == id);

            if (artist == null)
            {
                return NotFound();
            }

            var albumsDB = _context.AlbumsArtists
                .Where(a => a.ArtistId == id)
                .Join
                (
                    _context.Album
                    .Include(c => c.AlbumsArtists)
                        .ThenInclude(g => g.Artist),
                    g => g.AlbumId,
                    a => a.AlbumId,
                    (g, a) => new { g, a }
                )
                .Select(ga => ga.a);

            var albums = await albumsDB.ToListAsync();

            ArtistsViewModel artistsView = new ArtistsViewModel();
            if (User.Identity.IsAuthenticated)
            {
                var currentUserId = new Guid(User.FindFirst(x => x.Type == "id").Value);
                artistsView = ViewModelConstructor.FavouriteArtists(new List<Artist>() { artist }, _context, currentUserId).First();
            }
            else
            {
                artistsView.Artist = artist;
            }
            artistsView.Albums = albums;
            return View(artistsView);
        }

        // GET: Artists/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Artists/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ArtistId,Name,Information")] Artist artist)
        {
            if (ModelState.IsValid)
            {
                artist.ArtistId = Guid.NewGuid();
                _context.Add(artist);
                await _context.SaveChangesAsync();
                return RedirectToAction("Details", new { id = artist.ArtistId });
            }
            return View(artist);
        }

        // GET: Artists/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var artist = await _context.Artist.FindAsync(id);
            if (artist == null)
            {
                return NotFound();
            }
            return View(artist);
        }

        // POST: Artists/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("ArtistId,Name,Information")] Artist artist)
        {
            if (id != artist.ArtistId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(artist);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ArtistExists(artist.ArtistId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("Details", new { id = artist.ArtistId });
            }
            return View(artist);
        }

        // GET: Artists/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var artist = await _context.Artist
                .FirstOrDefaultAsync(m => m.ArtistId == id);
            if (artist == null)
            {
                return NotFound();
            }

            return View(artist);
        }

        // POST: Artists/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var artist = await _context.Artist.FindAsync(id);
            _context.Artist.Remove(artist);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index","Search");
        }

        private bool ArtistExists(Guid id)
        {
            return _context.Artist.Any(e => e.ArtistId == id);
        }
    }
}
