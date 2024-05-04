using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Accountool.Models
{
    public class UserKiosk
    {
        [ForeignKey("User")]
        public string UserId { get; set; }

        [ForeignKey("Kiosk")]
        public int KioskId { get; set; }

        public virtual AspNetUser User { get; set; }
        public virtual Kiosk Kiosk { get; set; }
    }
}