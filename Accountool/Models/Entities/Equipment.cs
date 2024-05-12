using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Accountool.Models.Entities
{
    [Table("Equipment")]
    public partial class Equipment
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public string ModelEq { get; set; } = null!;

        [Required]
        public string Description { get; set; } = null!;

        public int PowerEq { get; set; }

        [Required]
        [ForeignKey("Places")]
        public int PlaceId { get; set; }

        [InverseProperty("Equipments")]
        public virtual Place Place { get; set; } = null!;
    }
}