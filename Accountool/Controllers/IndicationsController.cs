using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Accountool.Data;
using Accountool.Models.Entities;
using Accountool.Models.Models;
using System.Diagnostics.Metrics;
using Accountool.Models.Services;

namespace Accountool.Controllers
{
    public class IndicationsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IMeasurementService _measurementService;

        public IndicationsController(
            ApplicationDbContext context,
            IMeasurementService measurementService)
        {
            _context = context;
            _measurementService = measurementService;
        }

        // GET: Indications
        public async Task<IActionResult> Index(int MeasureTypeId)
        {
            var datas = await _measurementService.GetFilteredMeasures(MeasureTypeId);
            return View(datas);
        }

        // GET: Indications/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var indication = await _context.Indications
                .Include(i => i.Schetchik)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (indication == null)
            {
                return NotFound();
            }

            return View(indication);
        }

        // GET: Indications/Create
        public IActionResult Create()
        {
            ViewData["SchetchikId"] = new SelectList(_context.Schetchiks, "Id", "ModelSchetchika");
            return View();
        }

        // POST: Indications/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Month,Tarif1,Tarif2,TarifSumm,Archive,SchetchikId")] Indication indication)
        {
            if (ModelState.IsValid)
            {
                _context.Add(indication);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["SchetchikId"] = new SelectList(_context.Schetchiks, "Id", "ModelSchetchika", indication.SchetchikId);
            return View(indication);
        }

        // GET: Indications/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var indication = await _context.Indications.FindAsync(id);
            if (indication == null)
            {
                return NotFound();
            }
            ViewData["SchetchikId"] = new SelectList(_context.Schetchiks.Distinct(), "Id", "ModelSchetchika", indication.SchetchikId);
            return View(indication);
        }

        // POST: Indications/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Month,Tarif1,Tarif2,TarifSumm,Archive,SchetchikId")] Indication indication)
        {
            if (id != indication.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(indication);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!IndicationExists(indication.Id))
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
            ViewData["SchetchikId"] = new SelectList(_context.Schetchiks, "Id", "ModelSchetchika", indication.SchetchikId);
            return View(indication);
        }

        // GET: Indications/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var indication = await _context.Indications
                .Include(i => i.Schetchik)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (indication == null)
            {
                return NotFound();
            }

            return View(indication);
        }

        // POST: Indications/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var indication = await _context.Indications.FindAsync(id);
            if (indication != null)
            {
                _context.Indications.Remove(indication);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool IndicationExists(int id)
        {
            return _context.Indications.Any(e => e.Id == id);
        }
    }
}
