using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MusicOn.Logic;
using MusicOn.Models;
using MusicOn.ViewModels;

namespace MusicOn.Controllers
{
    public class SearchController : Controller
    {
        private readonly MusicOnContext _context;

        public SearchController(MusicOnContext context)
        {
            _context = context;
        }
        // GET: SearchResults
        public async Task<IActionResult> Index(string searchString)
        {
            var search = new SearchModel();
            var compositions = from c in _context.Composition
                               .Include(c => c.Album.AlbumsArtists)
                               .ThenInclude(c => c.Artist)
                               .Include(c => c.Song)
                               select c;

            if (!String.IsNullOrEmpty(searchString))
            {
                compositions = compositions.Where(s => s.Song.Title.Contains(searchString));
            }
            var compositionsF = await compositions.ToListAsync();
            if (User.Identity.IsAuthenticated)
            {
                var userId = new Guid(User.FindFirst(x => x.Type == "id").Value);
                search.Compositions = ViewModelConstructor.FavouriteSongs(compositionsF, _context, userId);
            }
            else
            {
                foreach (var comp in compositionsF)
                {
                    var compVM = new CompositionViewModel() { Composition = comp };
                    search.Compositions.Add(compVM);
                }
            }
            var artists = from c in _context.Artist
                               .Include(c=>c.AlbumsArtists)
                               .ThenInclude(c=>c.Album)
                               .ThenInclude(c=>c.Genre)
                               select c;

            if (!String.IsNullOrEmpty(searchString))
            {
                artists = artists.Where(s => s.Name.Contains(searchString));
            }
            var artistsF = await artists.ToListAsync();
            if (User.Identity.IsAuthenticated)
            {
                var userId = new Guid(User.FindFirst(x => x.Type == "id").Value);
                search.Artists = ViewModelConstructor.FavouriteArtists(artistsF, _context, userId);
            }
            else
            {
                foreach (var art in artistsF)
                {
                    var artVM = new ArtistsViewModel() { Artist = art };
                    search.Artists.Add(artVM);
                }
            }
            
            var albums = from c in _context.Album
                               .Include(c => c.AlbumsArtists)
                               .ThenInclude(c => c.Artist)
                          select c;

            if (!String.IsNullOrEmpty(searchString))
            {
                albums = albums.Where(s => s.Title.Contains(searchString));
            }
            search.Albums = await albums.ToListAsync();
            var mixTapes = from c in _context.MixTape
                         select c;

            if (!String.IsNullOrEmpty(searchString))
            {
                mixTapes = mixTapes.Where(s => s.Title.Contains(searchString));
            }
            search.MixTapes = await mixTapes.ToListAsync();
            return View(search);
        }
    }
}