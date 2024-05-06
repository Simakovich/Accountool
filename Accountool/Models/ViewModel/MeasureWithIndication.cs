using Accountool.Models.Entities;

namespace Accountool.Models.ViewModel
{
    public class MeasureWithIndication
    {
        public IEnumerable<MeasureType> MeasureTypes { get; set; } = new List<MeasureType>();

        public virtual IEnumerable<Indication> Indications { get; set; } = new List<Indication>();
    }
}
