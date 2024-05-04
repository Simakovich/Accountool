using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Accountool.Models
{
    [Table("Indication")]
    public partial class Indication
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public DateTime Month { get; set; }

        [Required]
        public double Tarif1 { get; set; }

        [Required]
        public double Tarif2 { get; set; }

        [Required]
        public double TarifSumm { get; set; }

        public bool Archive { get; set; }

        [Required]
        [ForeignKey("Schetchiks")]
        public int SchetchikId { get; set; }

        [InverseProperty("Indications")]
        public virtual Schetchik Schetchik { get; set; } = null!;
    }
}