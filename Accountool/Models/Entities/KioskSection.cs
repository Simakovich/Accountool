using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;

namespace Accountool.Models.Entities
{
    [Table("PlaceSection")]
    public partial class PlaceSection
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public string PlaceName { get; set; } = null!;

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

        [InverseProperty("PlaceSection")]
        public virtual ICollection<Place> Places { get; set; } = new List<Place>();
    }
}