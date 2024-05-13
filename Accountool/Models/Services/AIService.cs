using System;
using System.Diagnostics;
using System.Linq;
using System.Security.Cryptography.Xml;
using Accountool.Models.DataAccess;
using Accountool.Models.Entities;
using Microsoft.CodeAnalysis.Diagnostics;
using Microsoft.EntityFrameworkCore;
using Microsoft.ML;
using Microsoft.ML.Data;
using Microsoft.ML.Trainers.FastTree;
using Microsoft.ML.Transforms;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Accountool.Models.Services
{
    public interface IAIService
    {
        Task<ConsumptionPrediction> SinglePrediction(int measureTypeId = 1);
        Task<IEnumerable<ConsumptionPrediction>> PredictedMeterReadings(int measureTypeId = 1);
    }

    public class AIService : IAIService
    {
        private readonly IRepository<Indication> _indications;
        private readonly IRepository<MeasureType> _measureTypes;
        private readonly IRepository<Schetchik> _schetchiks;
        private readonly IRepository<Place> _places;
        public AIService(
                IRepository<Indication> indications, 
                IRepository<MeasureType> measureTypes,
                IRepository<Schetchik> schetchiks,
                IRepository<Place> places)
        {
            _indications = indications;
            _measureTypes = measureTypes;
            _schetchiks = schetchiks;
            _places = places;
        }
        
        public async Task<ConsumptionPrediction> SinglePrediction(int measureTypeId = 1)
        {
            var mlContext = new MLContext();
            var indications = from mt in _measureTypes.GetAll()
                              join s in _schetchiks.GetAll() on mt.Id equals s.MeasureTypeId
                              join i in _indications.GetAll() on s.Id equals i.SchetchikId
                              join k in _places.GetAll() on s.PlaceId equals k.Id
                              where mt.Id == measureTypeId
                              select new ConsumptionData { Month = i.Month.Month, Label = Convert.ToSingle(i.Value), MeasurementObjectId = k.Id };

            var data = mlContext.Data.LoadFromEnumerable(indications.ToList());

            var processedData = mlContext.Transforms.CustomMapping(
                    (Action<ConsumptionData, ConsumptionData>)Mapping,
                    contractName: "CleanupMapping"
                ).Fit(data).Transform(data);

            processedData = mlContext.Transforms.ReplaceMissingValues(
                "Label",
                replacementMode: MissingValueReplacingEstimator.ReplacementMode.Mean
            ).Fit(processedData).Transform(processedData);

            //// var enumerableData = mlContext.Data.CreateEnumerable<ConsumptionData>(processedData, reuseRowObject: false);
            //foreach (var d in enumerableData)
            //{
            //    Debug.WriteLine(d.Month.ToString() + ' ' + d.Label.ToString() + ' ' + d.MeasurementObjectId);
            //}
            // Определите пайплайн для прогнозирования.
            var pipeline = mlContext.Transforms.Categorical.OneHotEncoding("Month")
                .Append(mlContext.Transforms.Categorical.OneHotEncoding("MeasurementObjectId"))
                .Append(mlContext.Transforms.Concatenate("Features", "Month", "MeasurementObjectId"))
                .Append(mlContext.Transforms.NormalizeMinMax("Features"))
                .Append(mlContext.Regression.Trainers.FastTree())
                .AppendCacheCheckpoint(mlContext);

            var model = pipeline.Fit(processedData);

            // Предсказываем следующие показания.
            var predictionFunction = mlContext.Model.CreatePredictionEngine<ConsumptionData, ConsumptionPrediction>(model);

            // В качестве параметра передайте месяц, для которого вы хотите получить прогноз
            var singlePrediction = predictionFunction.Predict(new ConsumptionData() { Month = 2, MeasurementObjectId = 0 });
            return singlePrediction;
        }

        public async Task<IEnumerable<ConsumptionPrediction>> PredictedMeterReadings(int measureTypeId = 1)
        {
            var mlContext = new MLContext();
            var indications = from mt in _measureTypes.GetAll()
                              join s in _schetchiks.GetAll() on mt.Id equals s.MeasureTypeId
                              join i in _indications.GetAll() on s.Id equals i.SchetchikId
                              join k in _places.GetAll() on s.PlaceId equals k.Id
                              where mt.Id == measureTypeId
                              select new ConsumptionData { Month = i.Month.Month, Label = Convert.ToSingle(i.Value) };

            var data = mlContext.Data.LoadFromEnumerable(indications.ToList());

            var processedData = mlContext.Transforms.CustomMapping(
                    (Action<ConsumptionData, ConsumptionData>)Mapping,
                    contractName: "CleanupMapping"
                ).Fit(data).Transform(data);

            processedData = mlContext.Transforms.ReplaceMissingValues(
                "Label",
                replacementMode: MissingValueReplacingEstimator.ReplacementMode.Mean
            ).Fit(processedData).Transform(processedData);

            // Определите пайплайн для прогнозирования.
            var pipeline = mlContext.Transforms.Categorical.OneHotEncoding("Month")
                .Append(mlContext.Transforms.Concatenate("Features", "Month", "MeasurementObject"))
                .Append(mlContext.Transforms.NormalizeMinMax("Features"))
                .Append(mlContext.Regression.Trainers.FastTree())
                .AppendCacheCheckpoint(mlContext);

            // Заметьте, что 'Label' используется в качестве столбца для предсказания при обучении модели.
            // Предсказываем следующие показания.
            var model = pipeline.Fit(processedData);
            var prediction = model.Transform(processedData);
            var predictedMeterReadings = mlContext.Data.CreateEnumerable<ConsumptionPrediction>(prediction, reuseRowObject: false);
            return predictedMeterReadings;
        }

        private static void Mapping(ConsumptionData input, ConsumptionData output)
        {
            output.Month = input.Month;
            output.MeasurementObjectId = input.MeasurementObjectId;
            if (input.Label <= 0.0f || input.Label > 1000.0f)
            {
                output.Label = float.NaN;
            }
            else
            {
                output.Label = input.Label;
            }
        }
    }

    public class ConsumptionData
    {
        [LoadColumn(0)] public int Month;
        [LoadColumn(1)] public float Label;
        [LoadColumn(2)] public int MeasurementObjectId;
    }
    
    public class ConsumptionPrediction
    {
        [ColumnName("Score")] public float PredictedLabel;
    }
}