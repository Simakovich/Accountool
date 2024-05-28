﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace Accountool.Models.Entities
{
    [Table("AspNetUser")]
    public partial class AspNetUser : IdentityUser
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string Id { get; set; } = null!;

        public string? UserName { get; set; }

        public string? NormalizedUserName { get; set; }

        public string? Email { get; set; }

        public string? NormalizedEmail { get; set; }

        public bool EmailConfirmed { get; set; }

        public string? PasswordHash { get; set; }

        public string? SecurityStamp { get; set; }

        public string? ConcurrencyStamp { get; set; }

        public string? PhoneNumber { get; set; }

        public bool PhoneNumberConfirmed { get; set; }

        public bool TwoFactorEnabled { get; set; }

        public DateTimeOffset? LockoutEnd { get; set; }

        public bool LockoutEnabled { get; set; }

        public int AccessFailedCount { get; set; }

        public virtual ICollection<AspNetUserClaim> AspNetUserClaims { get; set; } = new List<AspNetUserClaim>();

        public virtual ICollection<AspNetUserLogin> AspNetUserLogins { get; set; } = new List<AspNetUserLogin>();

        public virtual ICollection<AspNetUserToken> AspNetUserTokens { get; set; } = new List<AspNetUserToken>();

        public virtual ICollection<ClientProfile> ClientProfiles { get; set; } = new List<ClientProfile>();

        public virtual ICollection<Place> Places { get; set; } = new List<Place>();

        public virtual ICollection<UserPlace> UserPlaces { get; set; } = new List<UserPlace>();

        [InverseProperty("Users")]
        public virtual ICollection<AspNetRole> Roles { get; set; } = new List<AspNetRole>();

        [InverseProperty("User")]
        public virtual ICollection<AspNetUserRole> UserRoles { get; set; } = new List<AspNetUserRole>();
    }
}