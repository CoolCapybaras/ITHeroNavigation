using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace Domain.Models;

public class User
{
    [Key]
    public Guid Id { get; set; }

    [Required]
    public string Username { get; set; }

    [Required]
    [EmailAddress]
    public string Email { get; set; }

    [Required]
    public string HashPassword { get; set; }

    public ICollection<Favorite> Favorites { get; set; } = new List<Favorite>();

}