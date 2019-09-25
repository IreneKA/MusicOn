using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MusicOn.Logic;
using MusicOn.Models;
using MusicOn.ViewModels;

namespace MusicOn.Controllers
{
    public class HomeController : Controller
    {
        private readonly MusicOnContext _context;

        public HomeController(MusicOnContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index()
        {
            var compositions = from c in _context.Composition
                               .Include(c => c.Album.AlbumsArtists)
                               .ThenInclude(c => c.Artist)
                               .Include(c => c.Song)
                               .OrderByDescending(c => c.FavouriteSongs.Count)
                               .Take(10)
                               select c;
            var albums = from c in _context.Album
                               .Include(c => c.AlbumsArtists)
                               .ThenInclude(c => c.Artist)
                               .OrderByDescending(c => c.Date)
                               .Take(10)
                         select c;

            var compositionsF = await compositions.ToListAsync();

            var home = new HomeModel
            {
                Albums = await albums.ToListAsync(),
            };
            if (User.Identity.IsAuthenticated)
            {
                var userId = new Guid(User.FindFirst(x => x.Type == "id").Value);
                home.Compositions = ViewModelConstructor.FavouriteSongs(compositionsF, _context, userId);
            }
            else
            {
                foreach(var comp in compositionsF)
                {
                    var compVM = new CompositionViewModel() { Composition = comp };
                    home.Compositions.Add(compVM);
                }
            }
            //var userId = new Guid(User.FindFirst(x => x.Type == "id").Value);
            //home.Compositions = ViewModelConstructor.FavouriteSongs(compositionsF, _context, userId);

            return View(home);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
