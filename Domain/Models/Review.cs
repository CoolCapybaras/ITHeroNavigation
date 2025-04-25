using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Domain.Models;

public class Review
{
    [Key]
    public Guid Id { get; set; }
    
    [Required]
    public Guid AuthorId { get; set; }
    
    [ForeignKey("AuthorId")]
    [JsonIgnore]
    public User Author { get; set; }
    
    [Range(0, 5)]
    public double Rating { get; set; }
    
    public string Text { get; set; }
    
    public Guid PlaceId { get; set; }
    
    [ForeignKey("PlaceId")]
    [JsonIgnore]
    public Place Place { get; set; }
    
    public DateTime AddedAt { get; set; } = DateTime.UtcNow;
}