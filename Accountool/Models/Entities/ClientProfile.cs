using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace Accountool.Models.Entities
{
    [Table("ClientProfile")]
    public partial class ClientProfile
    {
        public string UserId { get; set; }
        public int PlaceId { get; set; }
        public virtual Place Place { get; set; }

        ///[InverseProperty("ClientProfiles")]
        public IdentityUser User { get; set; }
    }
}