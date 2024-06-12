using Accountool.Models.Entities;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

public class UserPlace
{
    public string UserId { get; set; }
    public int PlaceId { get; set; }

    public IdentityUser User { get; set; }
    public Place Place { get; set; }
}