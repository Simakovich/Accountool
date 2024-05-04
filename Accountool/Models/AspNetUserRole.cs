using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Accountool.Models
{
    [Table("CustomUserRoles")]
    public class AspNetUserRole
    {
        [ForeignKey("AspNetUsers")]
        public string UserId { get; set; }

        [ForeignKey("AspNetRoles")]
        public string RoleId { get; set; }

        public virtual AspNetUser User { get; set; }
        public virtual AspNetRole Role { get; set; }
    }
}