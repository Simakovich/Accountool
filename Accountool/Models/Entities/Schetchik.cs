using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;

namespace Accountool.Models.Entities
{
    [Table("Schetchik")]
    public partial class Schetchik
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public string NomerSchetchika { get; set; } = null!;

        [Required]
        public string ModelSchetchika { get; set; } = null!;

        public bool TexUchet { get; set; }

        public bool TwoTarif { get; set; }

        public DateTime? Poverka { get; set; }

        [Required]
        public int Poteri { get; set; }

        [ForeignKey("Place")]
        public int? PlaceId { get; set; }

        [ForeignKey("Schetchik")]
        public int MeasureTypeId { get; set; }

        [InverseProperty("Schetchik")]
        public virtual ICollection<Indication> Indications { get; set; } = new List<Indication>();

        [InverseProperty("Schetchiks")]
        public virtual Place? Place { get; set; }

        [InverseProperty("Schetchiks")]
        public virtual MeasureType MeasureType { get; set; }
    }
}