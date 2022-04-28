#nullable disable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using JokesApp.Data;
using JokesApp.Models;
using Microsoft.AspNetCore.Authorization;

namespace JokesApp.Controllers
{
    public class SakalarController : Controller
    {
        private readonly ApplicationDbContext _context;

        public SakalarController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Sakas
        public async Task<IActionResult> Index()
        {
            return View(await _context.Saka.ToListAsync());
        }
        
        // GET: Sakalar/Search
        public async Task<IActionResult> Search()
        {
            return View();
        }
        
        // PoST: Sakalar/SearchResults
        public async Task<IActionResult> SearchResults(string SearchPhrase)
        {
            return View("Index", await _context.Saka.Where(j => j.SakaSoru.Contains(SearchPhrase)).ToListAsync());

        }

        // GET: Sakas/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var saka = await _context.Saka
                .FirstOrDefaultAsync(m => m.Id == id);
            if (saka == null)
            {
                return NotFound();
            }

            return View(saka);
        }

        // GET: Sakas/Create
        [Authorize]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Sakas/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Create([Bind("Id,SakaSoru,SakaCevap")] Saka saka)
        {
            if (ModelState.IsValid)
            {
                _context.Add(saka);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(saka);
        }

        // GET: Sakas/Edit/5
        [Authorize]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var saka = await _context.Saka.FindAsync(id);
            if (saka == null)
            {
                return NotFound();
            }
            return View(saka);
        }

        // POST: Sakas/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Edit(int id, [Bind("Id,SakaSoru,SakaCevap")] Saka saka)
        {
            if (id != saka.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(saka);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SakaExists(saka.Id))
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
            return View(saka);
        }

        // GET: Sakas/Delete/5
        [Authorize]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var saka = await _context.Saka
                .FirstOrDefaultAsync(m => m.Id == id);
            if (saka == null)
            {
                return NotFound();
            }

            return View(saka);
        }

        // POST: Sakas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var saka = await _context.Saka.FindAsync(id);
            _context.Saka.Remove(saka);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool SakaExists(int id)
        {
            return _context.Saka.Any(e => e.Id == id);
        }
    }
}
