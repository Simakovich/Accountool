using Accountool.Models.Entities;
using Accountool.Models.Models;
using Accountool.Utils;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Accountool.Models.ViewModel
{
    public class MeasureWithOneIndication : MeasureWithIndication
    {
        public MeasureWithOneIndication(MeasureWithIndication measureWithIndication)
            : base(measureWithIndication)
        {
        }
        public MeasureWithOneIndication()
        {
        }
        public  FullIndicationModel FullIndicationModel { get; set; } = new FullIndicationModel();
    }
}
