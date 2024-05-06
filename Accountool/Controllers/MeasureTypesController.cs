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
    public class MeasureTypesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public MeasureTypesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: MeasureTypes
        public async Task<IActionResult> Index()
        {
            return View(await _context.MeasureTypes.ToListAsync());
        }

        // GET: MeasureTypes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var measureType = await _context.MeasureTypes
                .FirstOrDefaultAsync(m => m.Id == id);
            if (measureType == null)
            {
                return NotFound();
            }

            return View(measureType);
        }

        // GET: MeasureTypes/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: MeasureTypes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name")] MeasureType measureType)
        {
            if (ModelState.IsValid)
            {
                _context.Add(measureType);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(measureType);
        }

        // GET: MeasureTypes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var measureType = await _context.MeasureTypes.FindAsync(id);
            if (measureType == null)
            {
                return NotFound();
            }
            return View(measureType);
        }

        // POST: MeasureTypes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name")] MeasureType measureType)
        {
            if (id != measureType.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(measureType);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MeasureTypeExists(measureType.Id))
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
            return View(measureType);
        }

        // GET: MeasureTypes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var measureType = await _context.MeasureTypes
                .FirstOrDefaultAsync(m => m.Id == id);
            if (measureType == null)
            {
                return NotFound();
            }

            return View(measureType);
        }

        // POST: MeasureTypes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var measureType = await _context.MeasureTypes.FindAsync(id);
            if (measureType != null)
            {
                _context.MeasureTypes.Remove(measureType);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool MeasureTypeExists(int id)
        {
            return _context.MeasureTypes.Any(e => e.Id == id);
        }
    }
}
