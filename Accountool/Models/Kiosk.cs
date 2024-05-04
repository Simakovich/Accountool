﻿using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;

namespace Accountool.Models
{
    [Table("Kiosk")]
    public partial class Kiosk
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public string Nomer { get; set; } = null!;

        public string? ModelKiosk { get; set; }

        public DateTime? Arenda { get; set; }

        [ForeignKey("Towns")]
        public int? TownId { get; set; }

        public string? Adress { get; set; }

        public double Area { get; set; }

        [ForeignKey("Organizations")]
        public int? OrganizationId { get; set; }

        [ForeignKey("KioskSections")]
        public int? KioskSectionId { get; set; }

        [InverseProperty("Kiosk")]
        public virtual ICollection<Equipment> Equipments { get; set; } = new List<Equipment>();

        [InverseProperty("Kiosks")]
        public virtual KioskSection? KioskSection { get; set; }

        [InverseProperty("Kiosks")]
        public virtual Organization? Organization { get; set; }

        [InverseProperty("Kiosk")]
        public virtual ICollection<Schetchik> Schetchiks { get; set; } = new List<Schetchik>();

        [InverseProperty("Kiosk")]
        public virtual ICollection<UserKiosk> UserKiosks { get; set; } = new List<UserKiosk>();

        [InverseProperty("Kiosks")]
        public virtual Town? Town { get; set; }

        [InverseProperty("Kiosks")]
        public virtual ICollection<AspNetUser> Users { get; set; } = new List<AspNetUser>();
    }
}