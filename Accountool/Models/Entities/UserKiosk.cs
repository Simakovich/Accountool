using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Accountool.Models.Entities
{
    public class UserPlace
    {
        [ForeignKey("User")]
        public string UserId { get; set; }

        [ForeignKey("Place")]
        public int PlaceId { get; set; }

        public virtual AspNetUser User { get; set; }
        public virtual Place Place { get; set; }
    }
}