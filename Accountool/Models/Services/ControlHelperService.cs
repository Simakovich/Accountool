using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Accountool.Models.Entities;
using Accountool.Models.DataAccess;
using Accountool.Models.ViewModel;
using System.Data;
using Accountool.Models.Models;
using Humanizer;
using System;
using Accountool.Data;
using Microsoft.ML;
using Accountool.Utils;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc;

namespace Accountool.Models.Services
{
    public interface IControlHelperService
    {
        Task<MeasureWithIndication> GetAvailableValues(int measureTypeid);
    }

    public class ControlHelperService : IControlHelperService
    {
        private readonly IMeasurementService _measurementService;

        public ControlHelperService(
            IMeasurementService measurementService)
        {
            _measurementService = measurementService;
        }

        public async Task<MeasureWithIndication> GetAvailableValues(
            int measureTypeId)
        {
            var result = new MeasureWithIndication();
            await FillMeasureTypes(measureTypeId, result);
            await FillMinMaxYear(result);
            await FillMinMaxMonth(result);
            await FillPlaces(result);
            await FillTowns(result);
            return result;
        }

        private async Task FillMeasureTypes(int measureTypeId, MeasureWithIndication result)
        {
            result.MeasureTypes = await _measurementService.GetAllMeasureTypes();
            result.MeasureTypeName = result.MeasureTypes.FirstOrDefault(x => x.Id == measureTypeId)?.Name ?? string.Empty;
            result.MeasureTypeId = measureTypeId;
        }

        private async Task FillMinMaxYear(MeasureWithIndication measurements)
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
                                        })
                                        .ToList();
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

        private async Task FillTowns(MeasureWithIndication measurements)
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

    }
}