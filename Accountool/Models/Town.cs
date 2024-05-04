using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Accountool.Models;

[Table("Towns")]
public partial class Town
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    [Required]
    [Column("Name")]
    public string Name { get; set; } = null!;

    [InverseProperty("Town")]
    public virtual ICollection<Kiosk> Kiosks { get; set; } = new List<Kiosk>();
}