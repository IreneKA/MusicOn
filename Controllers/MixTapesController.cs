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
    public class MixTapesController : Controller
    {
        private readonly MusicOnContext _context;

        public MixTapesController(MusicOnContext context)
        {
            _context = context;
        }

        // GET: MixTapes
        public async Task<IActionResult> Index()
        {
            return View(await _context.MixTape.ToListAsync());
        }

        // GET: MixTapes/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var mixTape = await _context.MixTape
                .FirstOrDefaultAsync(m => m.MixTapeId == id);

            if (mixTape == null)
            {
                return NotFound();
            }

            var compositions = await _context.MixtapesCompositions
                .Where(mc => mc.MixTapeId == id)
                .Join
                (
                    _context.Composition
                    .Include(c => c.Album.AlbumsArtists)
                        .ThenInclude(ga => ga.Artist)
                    .Include(c => c.Song),
                    mc => mc.CompositionId,
                    c => c.CompositionId,
                    (mc, c) => new { mc, c }
                )
                .Select(mcc => mcc.c)
                .ToListAsync();

            MixTapeViewModel mixTapeView = new MixTapeViewModel();
            mixTapeView.MixTape = mixTape;
            if (User.Identity.IsAuthenticated)
            {
                var currentUserId = new Guid(User.FindFirst(x => x.Type == "id").Value);
                mixTapeView.Compositions = ViewModelConstructor.FavouriteSongs(compositions, _context, currentUserId);
            }
            else
            {
                foreach (var comp in compositions)
                {
                    var compVM = new CompositionViewModel() { Composition = comp };
                    mixTapeView.Compositions.Add(compVM);
                }
            }
            

            return View(mixTapeView);
        }

        // GET: MixTapes/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: MixTapes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("MixTapeId,Title,Description")] MixTape mixTape)
        {
            if (ModelState.IsValid)
            {
                mixTape.MixTapeId = Guid.NewGuid();
                _context.Add(mixTape);
                await _context.SaveChangesAsync();
                return RedirectToAction("Details", new { id = mixTape.MixTapeId });
            }
            return View(mixTape);
        }

        // GET: MixTapes/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var mixTape = await _context.MixTape.FindAsync(id);
            if (mixTape == null)
            {
                return NotFound();
            }
            return View(mixTape);
        }

        // POST: MixTapes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("MixTapeId,Title,Description")] MixTape mixTape)
        {
            if (id != mixTape.MixTapeId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(mixTape);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MixTapeExists(mixTape.MixTapeId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("Details", new { id = mixTape.MixTapeId });
            }
            return View(mixTape);
        }

        // GET: MixTapes/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var mixTape = await _context.MixTape
                .FirstOrDefaultAsync(m => m.MixTapeId == id);
            if (mixTape == null)
            {
                return NotFound();
            }

            return View(mixTape);
        }

        // POST: MixTapes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var mixTape = await _context.MixTape.FindAsync(id);
            _context.MixTape.Remove(mixTape);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index", "Search");
        }

        private bool MixTapeExists(Guid id)
        {
            return _context.MixTape.Any(e => e.MixTapeId == id);
        }
    }
}
