using Accountool.Models;
using Accountool.Models.Entities;
using Accountool.Models.Services;
using Accountool.Models.ViewModel;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Globalization;
using System.Text;

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
            return View("./Index", measurements);
        }


        // GET: Indications/Details/5
        public async Task<IActionResult> GetAllIndicationsByMeasure(int id)
        {
            var measurements = new MeasureWithIndication();
            measurements.MeasureTypes = await _measurementService.GetAllMeasureTypes();
            ViewBag.TitleMeasureType = measurements.MeasureTypes.FirstOrDefault(x => x.Id == id).Name;
            measurements.Indications = await _measurementService.GetByMeasureId(id);
            return View("./Index", measurements);

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
