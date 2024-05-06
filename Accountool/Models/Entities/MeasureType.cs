using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Accountool.Models.Entities;

[Table("MeasureType")]
public partial class MeasureType
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    [Required]
    [Column("Name")]
    public string Name { get; set; } = null!;

    [InverseProperty("MeasureType")]
    public virtual ICollection<Schetchik> Schetchiks { get; set; } = new List<Schetchik>();
}