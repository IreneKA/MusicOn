using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MusicOn.Models;

namespace MusicOn.Controllers
{
    public class AlbumsArtistsController : Controller
    {
        private readonly MusicOnContext _context;

        public AlbumsArtistsController(MusicOnContext context)
        {
            _context = context;
        }

        // GET: AlbumsArtists
        //public async Task<IActionResult> Index()
        //{
        //    var musicOnContext = _context.AlbumsArtists.Include(a => a.Album).Include(a => a.Artist);
        //    return View(await musicOnContext.ToListAsync());
        //}

        //// GET: AlbumsArtists/Details/5
        //public async Task<IActionResult> Details(Guid? id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        //    var albumsArtists = await _context.AlbumsArtists
        //        .Include(a => a.Album)
        //        .Include(a => a.Artist)
        //        .FirstOrDefaultAsync(m => m.ArtistId == id);
        //    if (albumsArtists == null)
        //    {
        //        return NotFound();
        //    }

        //    return View(albumsArtists);
        //}

        // GET: AlbumsArtists/Create
        public IActionResult Create(Guid? AlbumId,Guid? ArtistId)
        {
            ViewData["AlbumId"] = new SelectList(_context.Album, "AlbumId", "Title", AlbumId);
            ViewData["ArtistId"] = new SelectList(_context.Artist, "ArtistId", "Name", ArtistId);
            return View();
        }

        // POST: AlbumsArtists/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ArtistId,AlbumId")] AlbumsArtists albumsArtists)
        {
            if (ModelState.IsValid)
            {
                _context.Add(albumsArtists);
                await _context.SaveChangesAsync();
                return RedirectToAction("Details", "Albums", new { id = albumsArtists.AlbumId });
            }
            ViewData["AlbumId"] = new SelectList(_context.Album, "AlbumId", "Title", albumsArtists.AlbumId);
            ViewData["ArtistId"] = new SelectList(_context.Artist, "ArtistId", "Name", albumsArtists.ArtistId);
            return View(albumsArtists);
        }

        //// GET: AlbumsArtists/Edit/5
        //public async Task<IActionResult> Edit(Guid? id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        //    var albumsArtists = await _context.AlbumsArtists.FindAsync(id);
        //    if (albumsArtists == null)
        //    {
        //        return NotFound();
        //    }
        //    ViewData["AlbumId"] = new SelectList(_context.Album, "AlbumId", "Title", albumsArtists.AlbumId);
        //    ViewData["ArtistId"] = new SelectList(_context.Artist, "ArtistId", "Name", albumsArtists.ArtistId);
        //    return View(albumsArtists);
        //}

        //// POST: AlbumsArtists/Edit/5
        //// To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        //// more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Edit(Guid id, [Bind("ArtistId,AlbumId")] AlbumsArtists albumsArtists)
        //{
        //    if (id != albumsArtists.ArtistId)
        //    {
        //        return NotFound();
        //    }

        //    if (ModelState.IsValid)
        //    {
        //        try
        //        {
        //            _context.Update(albumsArtists);
        //            await _context.SaveChangesAsync();
        //        }
        //        catch (DbUpdateConcurrencyException)
        //        {
        //            if (!AlbumsArtistsExists(albumsArtists.ArtistId))
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
        //    ViewData["AlbumId"] = new SelectList(_context.Album, "AlbumId", "Title", albumsArtists.AlbumId);
        //    ViewData["ArtistId"] = new SelectList(_context.Artist, "ArtistId", "Name", albumsArtists.ArtistId);
        //    return View(albumsArtists);
        //}

        // GET: AlbumsArtists/Delete/5
        public async Task<IActionResult> Delete(Guid? ArtistId)
        {
            if (ArtistId == null)
            {
                return NotFound();
            }

            var albumsArtists = await _context.AlbumsArtists
                .Include(a => a.Album)
                .Include(a => a.Artist)
                .Where(m => m.ArtistId == ArtistId)
                .ToListAsync();
            if (albumsArtists == null)
            {
                return NotFound();
            }

            return View(albumsArtists);
        }

        // POST: AlbumsArtists/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid ArtistId, Guid AlbumId)
        {
            var albumsArtists = await _context
                .AlbumsArtists.Where(a=>a.ArtistId==ArtistId)
                .FirstOrDefaultAsync(a=>a.AlbumId==AlbumId);
            _context.AlbumsArtists.Remove(albumsArtists);
            await _context.SaveChangesAsync();

            return Redirect(Request.Headers["Referer"].ToString());
        }

        //private bool AlbumsArtistsExists(Guid id)
        //{
        //    return _context.AlbumsArtists.Any(e => e.ArtistId == id);
        //}
    }
}
