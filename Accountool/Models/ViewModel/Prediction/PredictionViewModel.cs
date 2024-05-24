using Accountool.Models.Entities;
using Microsoft.ML.Data;

namespace Accountool.Models.ViewModel.Prediction
{
    public class PredictionViewModel : MeasureWithOneIndication
    {
        public PredictionViewModel(MeasureWithIndication measureWithIndication)
            : base(measureWithIndication)
        {
        }
        public PredictionViewModel()
        {
        }
        public float? PredictedValue { get; set; }

    }
}
