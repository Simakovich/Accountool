using Accountool.Models.Entities;
using Accountool.Models.Models;
using Accountool.Utils;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Accountool.Models.ViewModel
{
    public class MeasureWithIndications : MeasureWithIndication
    {
        public MeasureWithIndications(MeasureWithIndication measureWithIndication)
            : base(measureWithIndication)
        {
        }
        public MeasureWithIndications()
        {
        }

        public PaginatedList<FullIndicationModel> FullIndicationModels { get; set; } = new PaginatedList<FullIndicationModel>();
    }
}
