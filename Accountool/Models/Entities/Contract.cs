using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;

namespace Accountool.Models.Entities
{
    [Table("Contract")]
    public partial class Contract
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [ForeignKey("Organization")]
        public int OrganizationId { get; set; }

        [Required]
        [ForeignKey("Place")]
        public int PlaceId { get; set; }

        [Required]
        public int Dogovor { get; set; }

        [Required]
        public int Limit { get; set; }

        [InverseProperty("Contracts")]
        public virtual Place Place { get; set; } = null!;

        [InverseProperty("Contracts")]
        public virtual Organization Organization { get; set; } = null!;
    }
}