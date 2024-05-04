using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;

namespace Accountool.Models
{
    [Table("KioskSection")]
    public partial class KioskSection
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public string NomerKioska { get; set; } = null!;

        [Required]
        public virtual string AdresSection { get; set; } = null!;

        public double AreaSection { get; set; }

        [Required]
        public string Kadastr { get; set; } = null!;

        [Required]
        public DateTime DataResh { get; set; }

        [Required]
        public string TypeArenda { get; set; } = null!;

        [Required]
        public string Certificate { get; set; } = null!;

        [Required]
        public DateTime DateArenda { get; set; }

        [InverseProperty("KioskSection")]
        public virtual ICollection<Kiosk> Kiosks { get; set; } = new List<Kiosk>();
    }
}