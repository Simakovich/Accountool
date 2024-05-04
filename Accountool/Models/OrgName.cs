using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;

namespace Accountool.Models
{
    [Table("OrgName")]
    public partial class OrgName
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; } = null!;

        [InverseProperty("OrgName")]
        public virtual ICollection<Organization> Organizations { get; set; } = new List<Organization>();
    }
}