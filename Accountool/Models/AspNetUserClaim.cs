using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Accountool.Models
{
    [Table("AspNetUserClaim")]
    public partial class AspNetUserClaim
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [ForeignKey("AspNetUsers")]
        public string UserId { get; set; } = null!;

        public string? ClaimType { get; set; }

        public string? ClaimValue { get; set; }

        [InverseProperty("AspNetUserClaims")]
        public virtual AspNetUser User { get; set; } = null!;
    }
}