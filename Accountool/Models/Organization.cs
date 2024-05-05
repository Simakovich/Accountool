using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;

namespace Accountool.Models
{
    [Table("Organization")]
    public partial class Organization
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; } = null!;

        [Required]
        public int Telefon { get; set; }

        [Required]
        public string Email { get; set; } = null!;

        [InverseProperty("Organization")]
        public virtual ICollection<Contract> Contracts { get; set; } = new List<Contract>();
    }
}