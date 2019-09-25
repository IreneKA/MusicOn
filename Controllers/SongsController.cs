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
    public class SongsController : Controller
    {
        private readonly MusicOnContext _context;

        public SongsController(MusicOnContext context)
        {
            _context = context;
        }

        // GET: Songs/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Songs/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("SongId,Title")] Song song)
        {
            if (ModelState.IsValid)
            {
                song.SongId = Guid.NewGuid();
                _context.Add(song);
                await _context.SaveChangesAsync();
                return Redirect(Request.Headers["Referer"].ToString());
            }
            return View(song);
        }
    }
}
