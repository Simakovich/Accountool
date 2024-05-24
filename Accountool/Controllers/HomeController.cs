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
        private readonly IAIService _aIService;
        
        public HomeController(
            ILogger<HomeController> logger,
            IMeasurementService measurementService,
            IAIService aIService)
        {
            _logger = logger;
            _measurementService = measurementService;
            _aIService = aIService;
        }

        public async Task<IActionResult> Index()
        {
            return View();
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
