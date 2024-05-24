using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Accountool.Data;
using Accountool.Models.Entities;

namespace Accountool.Controllers
{
    public class SchetchiksController : Controller
    {
        private readonly ApplicationDbContext _context;

        public SchetchiksController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Schetchiks
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Schetchiks.Include(s => s.MeasureType).Include(s => s.Place);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Schetchiks/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var schetchik = await _context.Schetchiks
                .Include(s => s.MeasureType)
                .Include(s => s.Place)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (schetchik == null)
            {
                return NotFound();
            }

            return View(schetchik);
        }

        // GET: Schetchiks/Create
        public IActionResult Create()
        {
            ViewData["MeasureTypeId"] = new SelectList(_context.MeasureTypes, "Id", "Name");
            ViewData["PlaceId"] = new SelectList(_context.Places, "Id", "Name");
            return View();
        }

        // POST: Schetchiks/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,NomerSchetchika,ModelSchetchika,TexUchet,TwoTarif,Poverka,Poteri,PlaceId,MeasureTypeId")] Schetchik schetchik)
        {
            if (ModelState.IsValid)
            {
                _context.Add(schetchik);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["MeasureTypeId"] = new SelectList(_context.MeasureTypes, "Id", "Name", schetchik.MeasureTypeId);
            ViewData["PlaceId"] = new SelectList(_context.Places, "Id", "Name", schetchik.PlaceId);
            return View(schetchik);
        }

        // GET: Schetchiks/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var schetchik = await _context.Schetchiks.FindAsync(id);
            if (schetchik == null)
            {
                return NotFound();
            }
            ViewData["MeasureTypeId"] = new SelectList(_context.MeasureTypes, "Id", "Name", schetchik.MeasureTypeId);
            ViewData["PlaceId"] = new SelectList(_context.Places, "Id", "Name", schetchik.PlaceId);
            return View(schetchik);
        }

        // POST: Schetchiks/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,NomerSchetchika,ModelSchetchika,TexUchet,TwoTarif,Poverka,Poteri,PlaceId,MeasureTypeId")] Schetchik schetchik)
        {
            if (id != schetchik.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(schetchik);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SchetchikExists(schetchik.Id))
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
            ViewData["MeasureTypeId"] = new SelectList(_context.MeasureTypes, "Id", "Name", schetchik.MeasureTypeId);
            ViewData["PlaceId"] = new SelectList(_context.Places, "Id", "Name", schetchik.PlaceId);
            return View(schetchik);
        }

        // GET: Schetchiks/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var schetchik = await _context.Schetchiks
                .Include(s => s.MeasureType)
                .Include(s => s.Place)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (schetchik == null)
            {
                return NotFound();
            }

            return View(schetchik);
        }

        // POST: Schetchiks/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var schetchik = await _context.Schetchiks.FindAsync(id);
            if (schetchik != null)
            {
                _context.Schetchiks.Remove(schetchik);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<JsonResult> GetCounters(int placeId)
        {
            var counters = await _context.Schetchiks
                .Include(s => s.MeasureType)
                .Include(s => s.Place)
                .Where(m => m.PlaceId == placeId)
                .ToListAsync();
            // Здесь предполагается получение списка счетчиков для указанного места (placeId)
            //var counters = new List<object>(); // Замените на фактическую логику получения счетчиков

            //// Пример заполнения списка
            //counters.Add(new { id = 1, name = "Counter 1" });
            //counters.Add(new { id = 2, name = "Counter 2" });

            var counterModel = counters.Select(x => (new { id = x.Id, name = x.NomerSchetchika }));
            return Json(counterModel); // Возвращаем список счетчиков в формате JSON
        }

        private bool SchetchikExists(int id)
        {
            return _context.Schetchiks.Any(e => e.Id == id);
        }
    }
}
