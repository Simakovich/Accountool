using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Accountool.Models.Entities
{
    [Table("AspNetUserLogin")]
    public partial class AspNetUserLogin
    {
        [Required]
        public string LoginProvider { get; set; } = null!;

        [Required]
        public string ProviderKey { get; set; } = null!;

        public string? ProviderDisplayName { get; set; }

        [Required]
        [ForeignKey("AspNetUsers")]
        public string UserId { get; set; } = null!;

        [InverseProperty("AspNetUserLogins")]
        public virtual AspNetUser User { get; set; } = null!;
    }
}
