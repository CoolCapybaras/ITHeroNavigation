using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Domain.Models;

public class Favorite
{
    [Key]
    public Guid UserId { get; set; }
    
    [ForeignKey("UserId")]
    public User User { get; set; }
    
    [Key]
    public Guid PlaceId { get; set; }
    
    [ForeignKey("PlaceId")]
    public Place Place { get; set; }
    
    public DateTime AddedAt { get; set; } = DateTime.UtcNow;
}