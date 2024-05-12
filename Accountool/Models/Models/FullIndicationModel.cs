using Accountool.Models.Entities;

namespace Accountool.Models.Models
{
    public class FullIndicationModel
    {
        public int KioskId { get; set; }
        public int MeasureTypeId { get; set; }

        public string KioskName { get; set; } = null!;

        public string? TownName { get; set; }

        public string? Address { get; set; }

        public Indication Indication { get; set; }

    }
}
