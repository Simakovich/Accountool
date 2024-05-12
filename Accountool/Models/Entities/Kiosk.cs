using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;

namespace Accountool.Models.Entities
{
    [Table("Place")]
    public partial class Place
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; } = null!;

        public string? ModelPlace { get; set; }

        public DateTime? Arenda { get; set; }

        public string? Address { get; set; }

        public double Area { get; set; }

        [ForeignKey("Towns")]
        public int? TownId { get; set; }

        [ForeignKey("PlaceSections")]
        public int? PlaceSectionId { get; set; }

        [InverseProperty("Places")]
        public virtual PlaceSection? PlaceSection { get; set; }

        [InverseProperty("Places")]
        public virtual Town? Town { get; set; }

        [InverseProperty("Place")]
        public virtual ICollection<Equipment> Equipments { get; set; } = new List<Equipment>();

        [InverseProperty("Place")]
        public virtual ICollection<Schetchik> Schetchiks { get; set; } = new List<Schetchik>();

        [InverseProperty("Place")]
        public virtual ICollection<UserPlace> UserPlaces { get; set; } = new List<UserPlace>();

        [InverseProperty("Place")]
        public virtual ICollection<Contract> Contracts { get; set; } = new List<Contract>();

        [InverseProperty("Places")]
        public virtual ICollection<AspNetUser> Users { get; set; } = new List<AspNetUser>();
    }
}