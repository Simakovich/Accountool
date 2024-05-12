using Accountool.Models.Entities;
using Accountool.Models.Models;
using Accountool.Utils;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Accountool.Models.ViewModel
{
    public class MeasureWithIndication
    {   
        public IEnumerable<MeasureType> MeasureTypes { get; set; } = new List<MeasureType>();

        public virtual PaginatedList<FullIndicationModel> Indications { get; set; } = new PaginatedList<FullIndicationModel>();
        
        public int MinYear { get; set; }
        
        public int MaxYear { get; set; }

        public string TitleMeasureTypeName { get; set; }

        public int? TitleMeasureTypeId { get; set; }
        public string SelectedStartYear { get; set; }
        public string SelectedLastYear { get; set; }
        public List<SelectListItem> Years { get; set; }
        public string SelectedStartMonth { get; set; }
        public string SelectedLastMonth { get; set; }
        public List<SelectListItem> Months { get; set; }
        public string SelectedPlace { get; set; }
        public List<SelectListItem> Places { get; set; }
        public string SelectTown { get; set; }
        public List<SelectListItem> Towns { get; set; }
    }
}
