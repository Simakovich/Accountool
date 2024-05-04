using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Accountool.Models
{
    [Table("AspNetUserToken")]
    public partial class AspNetUserToken
    {
        [Required]
        [ForeignKey("AspNetUsers")]
        public string UserId { get; set; } = null!;

        [Required]
        public string LoginProvider { get; set; } = null!;

        [Required]
        public string Name { get; set; } = null!;

        public string? Value { get; set; }

        [InverseProperty("AspNetUserTokens")]
        public virtual AspNetUser User { get; set; } = null!;
    }
}