using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;

namespace Accountool.Models.Entities
{
    [Table("Kiosk")]
    public partial class Kiosk
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; } = null!;

        public string? ModelKiosk { get; set; }

        public DateTime? Arenda { get; set; }

        public string? Address { get; set; }

        public double Area { get; set; }

        [ForeignKey("Towns")]
        public int? TownId { get; set; }

        [ForeignKey("KioskSections")]
        public int? KioskSectionId { get; set; }

        [InverseProperty("Kiosks")]
        public virtual KioskSection? KioskSection { get; set; }

        [InverseProperty("Kiosks")]
        public virtual Town? Town { get; set; }

        [InverseProperty("Kiosk")]
        public virtual ICollection<Equipment> Equipments { get; set; } = new List<Equipment>();

        [InverseProperty("Kiosk")]
        public virtual ICollection<Schetchik> Schetchiks { get; set; } = new List<Schetchik>();

        [InverseProperty("Kiosk")]
        public virtual ICollection<UserKiosk> UserKiosks { get; set; } = new List<UserKiosk>();

        [InverseProperty("Kiosk")]
        public virtual ICollection<Contract> Contracts { get; set; } = new List<Contract>();

        [InverseProperty("Kiosks")]
        public virtual ICollection<AspNetUser> Users { get; set; } = new List<AspNetUser>();
    }
}