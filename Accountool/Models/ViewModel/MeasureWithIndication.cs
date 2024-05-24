using Accountool.Models.Entities;
using Accountool.Models.Models;
using Accountool.Utils;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Accountool.Models.ViewModel
{
    public class MeasureWithIndication
    {
        public MeasureWithIndication()
        {
        }
        
        public MeasureWithIndication(MeasureWithIndication other)
        {
            if (other != null)
            {
                MeasureTypes = new List<MeasureType>(other.MeasureTypes);
                MinYear = other.MinYear;
                MaxYear = other.MaxYear;
                MeasureTypeName = other.MeasureTypeName;
                MeasureTypeId = other.MeasureTypeId;
                SelectedStartYear = other.SelectedStartYear;
                SelectedLastYear = other.SelectedLastYear;
                Years = other.Years;
                SelectedStartMonth = other.SelectedStartMonth;
                SelectedLastMonth = other.SelectedLastMonth;
                Months = other.Months;
                SelectedPlace = other.SelectedPlace;
                Places = other.Places;
                SelectTown = other.SelectTown;
                Towns = other.Towns;
            }
        }

        public IEnumerable<MeasureType> MeasureTypes { get; set; } = new List<MeasureType>();
        
        public int MinYear { get; set; }
        
        public int MaxYear { get; set; }

        public string MeasureTypeName { get; set; }

        public int? MeasureTypeId { get; set; }
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
