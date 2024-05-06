using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Accountool.Models.Entities
{
    [Table("ClientProfile")]
    public partial class ClientProfile
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public string Address { get; set; } = null!;

        [ForeignKey("AspNetUsers")]
        public string? UserId { get; set; }

        [InverseProperty("ClientProfiles")]
        public virtual AspNetUser? User { get; set; }
    }
}