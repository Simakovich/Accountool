using Accountool.Models.Services;
using Accountool.Models.ViewModel;
using Accountool.Models.ViewModel.Prediction;
using Microsoft.AspNetCore.Mvc;

namespace Accountool.Controllers
{
    public class PredictionController : Controller
    {
        private readonly IControlHelperService _controlHelperService;
        private readonly IMeasurementService _measurementService;
        private readonly IAIService _aIService;

        public PredictionController(
            IControlHelperService controlHelperService,
            IMeasurementService measurementService,
            IAIService aIService)
        {
            _controlHelperService = controlHelperService;
            _measurementService = measurementService;
            _aIService = aIService;
        }

        public async Task<IActionResult> Index()
        {
            var result = new PredictionViewModel();
            result.MeasureTypes = await _measurementService.GetAllMeasureTypes();
            return View("./Index", result);
        }

        public async Task<IActionResult> IndexWithMeasureType(int measureTypeId)
        {
            var measurementsValue = await _controlHelperService.GetAvailableValues(measureTypeId);
            var predictionViewModel = new PredictionViewModel(measurementsValue);
            predictionViewModel.MeasureTypes = await _measurementService.GetAllMeasureTypes();
            return View("./Index", predictionViewModel);
        }

        public async Task<IActionResult> GetPredictionFor(int measureTypeId, int month, int counterSelect)
        {
            var measurementsValue = await _controlHelperService.GetAvailableValues(measureTypeId);
            var predictionViewModel = new PredictionViewModel(measurementsValue);
            predictionViewModel.MeasureTypes = await _measurementService.GetAllMeasureTypes();
            predictionViewModel.MeasureTypeId = measureTypeId;

            var indicationsQueriable = await _measurementService.GetLastMeasureByCounter(measureTypeId, counterSelect);
            predictionViewModel.FullIndicationModel = indicationsQueriable;
            var predicatedValue = await _aIService.SinglePrediction(measureTypeId, month, counterSelect);
            predictionViewModel.PredictedValue = predicatedValue.PredictedLabel;
            return View("./Index", predictionViewModel);
        }
    }
}
