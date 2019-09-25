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
    public class CompositionsController : Controller
    {
        private readonly MusicOnContext _context;

        public CompositionsController(MusicOnContext context)
        {
            _context = context;
        }

        // GET: Compositions
        public async Task<IActionResult> Index(string searchString)
        {
            var compositions = new List<CompositionViewModel>();
            var compositionsF = await _context.Composition
                               .Include(c => c.Album.AlbumsArtists)
                               .ThenInclude(c => c.Artist)
                               .Include(c => c.Song)
                               .ToListAsync();

            foreach (var composition in compositionsF)
            {
                var compositionVM = new CompositionViewModel();
                compositionVM.Composition = composition;
                var favourit = await _context.FavouriteSongs
                    .Where(f => f.UserId == new Guid(User.FindFirst(x => x.Type == "id").Value))
                    .FirstOrDefaultAsync(f => f.CompositionId == composition.CompositionId);
                if (favourit != null)
                {
                    compositionVM.IsFavourite = true;
                }
                compositions.Add(compositionVM);
            }

            return View(compositions);
            //var musicOnContext = _context.Composition.Include(c => c.Album).Include(c => c.Song);
            //return View(await musicOnContext.ToListAsync());
        }

        // GET: Compositions/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var composition = await _context.Composition
                .Include(c => c.Album.AlbumsArtists)
                    .ThenInclude(ga => ga.Artist)
                .Include(c => c.Song)
                .FirstOrDefaultAsync(m => m.CompositionId == id);
            if (composition == null)
            {
                return NotFound();
            }
            CompositionViewModel compositionView = new CompositionViewModel();
            if (User.Identity.IsAuthenticated)
            {
                var currentUserId = new Guid(User.FindFirst(x => x.Type == "id").Value);
                compositionView = ViewModelConstructor.FavouriteSongs(new List<Composition> { composition }, _context, currentUserId).First();
            }
            else
            {
                compositionView.Composition = composition;
            }

            return View(compositionView);
        }

        // GET: Compositions/Create
        public async Task<IActionResult> Create()
        {
            AlbumArtistss artistss = new AlbumArtistss(await _context.Album.Include(a => a.AlbumsArtists).ThenInclude(a => a.Artist).ToListAsync());
            ViewData["AlbumId"] = new SelectList(artistss.Albums, "AlbumId", "Title");
            
            //ViewData["SongId"] = new SelectList(_context.Song, "SongId", "Title");
            return View();
        }

        // POST: Compositions/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("AlbumId,Title,Duration")] SongTitleComposition songComposition)
        {
            if (ModelState.IsValid)
            {
                Song song = new Song() { SongId = Guid.NewGuid(), Title = songComposition.Title};
                _context.Add(song);
                Composition composition = new Composition()
                {
                    CompositionId = Guid.NewGuid(),
                    SongId = song.SongId, AlbumId = songComposition.AlbumId,
                    Duration = songComposition.Duration };
                _context.Add(composition);
                
                await _context.SaveChangesAsync();
                return RedirectToAction("Details", new { id = composition.CompositionId });
            }
            AlbumArtistss artistss = new AlbumArtistss(await _context.Album.Include(a => a.AlbumsArtists).ThenInclude(a => a.Artist).ToListAsync());
            ViewData["AlbumId"] = new SelectList(artistss.Albums, "AlbumId", "Title");
            //ViewData["SongId"] = new SelectList(_context.Song, "SongId", "Title", composition.SongId);
            return View(songComposition);
        }

        // GET: Compositions/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var composition = await _context.Composition.FindAsync(id);
            if (composition == null)
            {
                return NotFound();
            }
            AlbumArtistss artistss = new AlbumArtistss(await _context.Album.Include(a => a.AlbumsArtists).ThenInclude(a => a.Artist).ToListAsync());
            ViewData["AlbumId"] = new SelectList(artistss.Albums, "AlbumId", "Title");
            ViewData["SongId"] = new SelectList(_context.Song, "SongId", "Title", composition.SongId);
            return View(composition);
        }

        // POST: Compositions/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("CompositionId,SongId,AlbumId,Duration")] Composition composition)
        {
            if (id != composition.CompositionId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(composition);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CompositionExists(composition.CompositionId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("Details", new { id = composition.CompositionId });

            }
            AlbumArtistss artistss = new AlbumArtistss(await _context.Album.Include(a => a.AlbumsArtists).ThenInclude(a => a.Artist).ToListAsync());
            ViewData["AlbumId"] = new SelectList(artistss.Albums, "AlbumId", "Title");
            ViewData["SongId"] = new SelectList(_context.Song, "SongId", "Title", composition.SongId);
            return View(composition);
        }

        // GET: Compositions/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var composition = await _context.Composition
                .Include(c => c.Album)
                .Include(c => c.Song)
                .FirstOrDefaultAsync(m => m.CompositionId == id);
            if (composition == null)
            {
                return NotFound();
            }

            return View(composition);
        }

        // POST: Compositions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var composition = await _context.Composition.FindAsync(id);
            _context.Composition.Remove(composition);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index", "Search");
        }

        private bool CompositionExists(Guid id)
        {
            return _context.Composition.Any(e => e.CompositionId == id);
        }
    }
}
