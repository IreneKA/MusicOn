using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MusicOn.Logic;
using MusicOn.Models;

namespace MusicOn.Controllers
{
    public class MixtapesCompositionsController : Controller
    {
        private readonly MusicOnContext _context;

        public MixtapesCompositionsController(MusicOnContext context)
        {
            _context = context;
        }

        // GET: MixtapesCompositions
        //public async Task<IActionResult> Index()
        //{
        //    var musicOnContext = _context.MixtapesCompositions.Include(m => m.Composition).Include(m => m.MixTape);
        //    return View(await musicOnContext.ToListAsync());
        //}

        //// GET: MixtapesCompositions/Details/5
        //public async Task<IActionResult> Details(Guid? id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        //    var mixtapesCompositions = await _context.MixtapesCompositions
        //        .Include(m => m.Composition)
        //        .Include(m => m.MixTape)
        //        .FirstOrDefaultAsync(m => m.MixTapeId == id);
        //    if (mixtapesCompositions == null)
        //    {
        //        return NotFound();
        //    }

        //    return View(mixtapesCompositions);
        //}

        // GET: MixtapesCompositions/Create
        public async Task<IActionResult> Create(Guid id)
        {
            if(id==null)
            {
                return NotFound("id==null");
            }

            var mixtape = await _context.MixTape.FindAsync(id);
            if (mixtape == null)
            {
                return NotFound("mixtape == null");
            }

            MixtapesCompositions mixtapesCompositions = new MixtapesCompositions
            {
                MixTape = mixtape,
                MixTapeId = mixtape.MixTapeId
            };

            CompositionSongs compositionSongs = new CompositionSongs(await _context.Composition.Include(c=>c.Song).Where(c=>c.MixtapesCompositions.Where(m=>m.MixTapeId==id).Count()==0).ToListAsync());
            ViewData["CompositionId"] = new SelectList(compositionSongs.Compositions, "CompositionId", "Song");
            return View(mixtapesCompositions);
        }

        // POST: MixtapesCompositions/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("MixTapeId,CompositionId")] MixtapesCompositions mixtapesCompositions)
        {
            if (ModelState.IsValid)
            {
                _context.Add(mixtapesCompositions);
                await _context.SaveChangesAsync();
                return RedirectToAction("Details","MixTapes",new { id = mixtapesCompositions.MixTapeId });
            }
            ViewData["CompositionId"] = new SelectList(_context.Composition.Include(c=>c.Song), "CompositionId", "Song.Title", mixtapesCompositions.CompositionId);
            var mixtape = await _context.MixTape.FindAsync(mixtapesCompositions.MixTapeId);
            mixtapesCompositions.MixTape = mixtape;
            //ViewData["MixTapeId"] = new SelectList(_context.MixTape, "MixTapeId", "Title", mixtapesCompositions.MixTapeId);
            return View(mixtapesCompositions);
        }

        //// GET: MixtapesCompositions/Edit/5
        //public async Task<IActionResult> Edit(Guid? id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        //    var mixtapesCompositions = await _context.MixtapesCompositions.FindAsync(id);
        //    if (mixtapesCompositions == null)
        //    {
        //        return NotFound();
        //    }
        //    ViewData["CompositionId"] = new SelectList(_context.Composition, "CompositionId", "CompositionId", mixtapesCompositions.CompositionId);
        //    ViewData["MixTapeId"] = new SelectList(_context.MixTape, "MixTapeId", "Title", mixtapesCompositions.MixTapeId);
        //    return View(mixtapesCompositions);
        //}

        //// POST: MixtapesCompositions/Edit/5
        //// To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        //// more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Edit(Guid id, [Bind("MixTapeId,CompositionId")] MixtapesCompositions mixtapesCompositions)
        //{
        //    if (id != mixtapesCompositions.MixTapeId)
        //    {
        //        return NotFound();
        //    }

        //    if (ModelState.IsValid)
        //    {
        //        try
        //        {
        //            _context.Update(mixtapesCompositions);
        //            await _context.SaveChangesAsync();
        //        }
        //        catch (DbUpdateConcurrencyException)
        //        {
        //            if (!MixtapesCompositionsExists(mixtapesCompositions.MixTapeId))
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
        //    ViewData["CompositionId"] = new SelectList(_context.Composition, "CompositionId", "CompositionId", mixtapesCompositions.CompositionId);
        //    ViewData["MixTapeId"] = new SelectList(_context.MixTape, "MixTapeId", "Title", mixtapesCompositions.MixTapeId);
        //    return View(mixtapesCompositions);
        //}

        // GET: MixtapesCompositions/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var mixtape = await _context.MixTape.FindAsync(id);
            if (mixtape == null)
            {
                return NotFound();
            }

            var mixtapesCompositions = await _context.MixtapesCompositions
                .Include(m => m.Composition.Song)
                .Include(m => m.MixTape)
                .Where(m => m.MixTapeId == id)
                .ToListAsync();

            if (mixtapesCompositions == null)
            {
                return NotFound();
            }

            return View(mixtapesCompositions);
        }

        // POST: MixtapesCompositions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid MixTapeId, Guid CompositionId)
        {
            var mixtapesCompositions = await _context.MixtapesCompositions
                .Where(m => m.MixTapeId == MixTapeId)
                .FirstOrDefaultAsync(m=>m.CompositionId== CompositionId);

            _context.MixtapesCompositions.Remove(mixtapesCompositions);
            await _context.SaveChangesAsync();

            return Redirect(Request.Headers["Referer"].ToString());
        }

        //private bool MixtapesCompositionsExists(Guid id)
        //{
        //    return _context.MixtapesCompositions.Any(e => e.MixTapeId == id);
        //}
    }
}
