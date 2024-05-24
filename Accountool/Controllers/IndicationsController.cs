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
using Accountool.Models.ViewModel;
using Accountool.Utils;

namespace Accountool.Controllers
{
    public class IndicationsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IMeasurementService _measurementService;
        private readonly IControlHelperService _controlHelperService;

        public IndicationsController(
            ApplicationDbContext context,
            IMeasurementService measurementService,
        IControlHelperService controlHelperService)
        {
            _context = context;
            _measurementService = measurementService;
            _controlHelperService = controlHelperService;
        }

        public async Task<IActionResult> Index()
        {
            var measurements = new MeasureWithIndications();
            measurements.MeasureTypes = await _measurementService.GetAllMeasureTypes();
            await FillMinMaxYear(measurements);
            await FillMinMaxMonth(measurements);
            return View("./../Measure/MasterPageMeasure", measurements);
        }

        public async Task<IActionResult> GetAllIndicationsByMeasure(
            int measureTypeId,
            int? SelectedPlace,
            int? SelectTown,
            int? pageNumber,
            int? SelectedStartYear = null,
            int? SelectedLastYear = null,
            int? SelectedStartMonth = null,
            int? SelectedLastMonth = null)
        {
            //var measureTypeId = id;
            //measurements.MeasureTypes = await _measurementService.GetAllMeasureTypes();
            //measurements.MeasureTypeName = measurements.MeasureTypes.FirstOrDefault(x => x.Id == measureTypeId)?.Name ?? string.Empty;
            //measurements.MeasureTypeId = measureTypeId;
            var measurementsValue = await _controlHelperService.GetAvailableValues(measureTypeId);
            var measurements = new MeasureWithIndications(measurementsValue);
            var indicationsQueriable = await _measurementService.GetFilteredMeasures(measureTypeId, SelectTown, SelectedPlace,
                SelectedStartMonth, SelectedLastMonth, SelectedStartYear, SelectedLastYear);
            measurements.FullIndicationModels =
                await PaginatedList<FullIndicationModel>.CreateAsync(indicationsQueriable, pageNumber ?? 1,
                    Constants.PageSize);
            //await FillMinMaxYear(measurements);
            //await FillMinMaxMonth(measurements);
            //await FillPlaces(measurements);
            //await FillTowns(measurements);
            return View("./../Measure/MasterPageMeasure", measurements);
        }

            private async Task FillMinMaxYear(MeasureWithIndications measurements)
        {
            if (measurements.MeasureTypeId.HasValue)
            {
                var minMaxYear = await _measurementService.GetMinMaxYear(measurements.MeasureTypeId.Value);
                if (minMaxYear != null)
                {
                    measurements.MinYear = (int)((int?)minMaxYear.FirstDate.Year ?? Constants.FirstDayCurrentMonth.Date.Year);
                    measurements.MaxYear = (int)((int?)minMaxYear.LastDate.Year ?? (int)Constants.LastDayCurrentMonth.Year);
                    measurements.Years = Enumerable.Range(measurements.MinYear, measurements.MaxYear - measurements.MinYear + 1)
                                        .Select(i => new SelectListItem
                                        {
                                            Value = i.ToString(),
                                            Text = i.ToString()
                                        }).ToList();
                    measurements.SelectedLastYear = measurements?.Years?.Last()?.Value?.ToString();
                }
            }
        }

        private async Task FillMinMaxMonth(MeasureWithIndications measurements)
        {
            measurements.Months = Enumerable.Range(Constants.FirstMonth, Constants.LastMonth - Constants.FirstMonth + 1)
                                .Select(i => new SelectListItem
                                {
                                    Value = i.ToString(),
                                    Text = i.ToString()
                                }).ToList();
            measurements.SelectedLastMonth = Constants.LastMonth.ToString();
        }

        private async Task FillPlaces(MeasureWithIndications measurements)
        {
            if (measurements.MeasureTypeId.HasValue)
            {
                var places = await _measurementService.GetPlaces(measurements.MeasureTypeId.Value);
                measurements.Places = places.Select(x => new { x.Id, x.Name })
                                    .Distinct()
                                    .Select(i => new SelectListItem
                                    {
                                        Value = i.Id.ToString(),
                                        Text = i.Name.ToString()
                                    }).ToList();
                measurements.Places.Insert(0, new SelectListItem { Value = "", Text = "Please select" });
                measurements.SelectedPlace = measurements?.Places?.FirstOrDefault()?.ToString();
            }
        }

        private async Task FillTowns(MeasureWithIndications measurements)
        {
            if (measurements.MeasureTypeId.HasValue)
            {
                var towns = await _measurementService.GetTowns(measurements.MeasureTypeId.Value);
                measurements.Towns = towns.Select(x => new { x.Id, x.Name })
                                    .Distinct()
                                    .Select(i => new SelectListItem
                                    {
                                        Value = i.Id.ToString(),
                                        Text = i.Name.ToString()
                                    }).ToList();
                measurements.Towns.Insert(0, new SelectListItem { Value = "", Text = "Please select" });
                measurements.SelectTown = measurements?.Towns?.FirstOrDefault()?.ToString();
            }
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
            ViewData["SchetchikId"] = new SelectList(_context.Schetchiks, "Id", "NomerSchetchika");
            return View();
        }

        // POST: Indications/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Month,Tarif1,Value,Archive,SchetchikId")] Indication indication)
        {
            ModelState.Remove("Schetchik");
            if (ModelState.IsValid)
            {
                _context.Add(indication);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["SchetchikId"] = new SelectList(_context.Schetchiks, "Id", "NomerSchetchika", indication.SchetchikId);
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
            ViewData["SchetchikId"] = new SelectList(_context.Schetchiks.Distinct(), "Id", "NomerSchetchika", indication.SchetchikId);
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

            ModelState.Remove("Schetchik");
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
            ViewData["SchetchikId"] = new SelectList(_context.Schetchiks, "Id", "NomerSchetchika", indication.SchetchikId);
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
