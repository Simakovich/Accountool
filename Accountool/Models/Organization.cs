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
        [ForeignKey("OrgName")]
        public int OrgNameId { get; set; }

        [Required]
        public int Dogovor { get; set; }

        [Required]
        public int Telefon { get; set; }

        [Required]
        public string Email { get; set; } = null!;

        [Required]
        public int Limit { get; set; }

        [InverseProperty("Organization")]
        public virtual ICollection<Kiosk> Kiosks { get; set; } = new List<Kiosk>();

        [InverseProperty("Organizations")]
        public virtual OrgName OrgName { get; set; } = null!;
    }
}