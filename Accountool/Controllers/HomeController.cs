using Accountool.Models;
using Accountool.Models.Entities;
using Accountool.Models.Models;
using Accountool.Models.Services;
using Accountool.Models.ViewModel;
using Accountool.Utils;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Diagnostics;
using System.Drawing.Printing;
using System.Globalization;
using System.Text;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Accountool.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IMeasurementService _measurementService;
        
        public HomeController(ILogger<HomeController> logger, IMeasurementService measurementService)
        {
            _logger = logger;
            _measurementService = measurementService;
        }

        public async Task<IActionResult> Index()
        {
            //var measurements = await _measurementService.GetAllIn();
            var measurements = new MeasureWithIndication();
            measurements.MeasureTypes = await _measurementService.GetAllMeasureTypes();
            await FillMinMaxYear(measurements);
            await FillMinMaxMonth(measurements);
            return View("./Index", measurements);
        }

        // GET: Indications/Details/5
        public async Task<IActionResult> GetAllIndicationsByMeasure(
            int id,
            int? SelectedPlace,
            int? SelectTown,
            int? pageNumber,
            int? SelectedStartYear = null, 
            int? SelectedLastYear = null, 
            int? SelectedStartMonth = null,
            int? SelectedLastMonth = null)
        {
            var measureTypeId = id;
            var measurements = new MeasureWithIndication();
            measurements.MeasureTypes = await _measurementService.GetAllMeasureTypes();
            measurements.TitleMeasureTypeName = measurements.MeasureTypes.FirstOrDefault(x => x.Id == measureTypeId)?.Name ?? string.Empty;
            measurements.TitleMeasureTypeId = measureTypeId;
            var indicationsQueriable = await _measurementService.GetFilteredMeasures(measureTypeId, SelectTown, SelectedPlace, SelectedStartMonth, SelectedLastMonth, SelectedStartYear, SelectedLastYear);
            measurements.Indications = await PaginatedList<FullIndicationModel>.CreateAsync(indicationsQueriable, pageNumber ?? 1, Constants.PageSize);
            await FillMinMaxYear(measurements);
            await FillMinMaxMonth(measurements);
            await FillPlaces(measurements);
            await FillTowns(measurements);
            return View("./Index", measurements);
        }

        private async Task FillMinMaxYear(MeasureWithIndication measurements)
        {
            if (measurements.TitleMeasureTypeId.HasValue)
            {
                var minMaxYear = await _measurementService.GetMinMaxYear(measurements.TitleMeasureTypeId.Value);
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

        private async Task FillMinMaxMonth(MeasureWithIndication measurements)
        {
            measurements.Months = Enumerable.Range(Constants.FirstMonth, Constants.LastMonth - Constants.FirstMonth + 1)
                                .Select(i => new SelectListItem
                                {
                                    Value = i.ToString(),
                                    Text = i.ToString()
                                }).ToList();
            measurements.SelectedLastMonth = Constants.LastMonth.ToString();
        }

        private async Task FillPlaces(MeasureWithIndication measurements)
        {
            if (measurements.TitleMeasureTypeId.HasValue)
            {
                var places = await _measurementService.GetPlaces(measurements.TitleMeasureTypeId.Value);
                measurements.Places = places.Select(x => new { x.Id, x.Name })
                                    .Distinct()
                                    .Select(i => new SelectListItem
                                    {
                                        Value = i.Id .ToString(),
                                        Text = i.Name.ToString()
                                    }).ToList();
                measurements.Places.Insert(0, new SelectListItem { Value = "", Text = "Please select" });
                measurements.SelectedPlace = measurements?.Places?.FirstOrDefault()?.ToString();
            }
        }

        private async Task FillTowns(MeasureWithIndication measurements)
        {
            if (measurements.TitleMeasureTypeId.HasValue)
            {
                var towns = await _measurementService.GetTowns(measurements.TitleMeasureTypeId.Value);
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

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
