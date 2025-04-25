using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Models;

public class Review
{
    [Key]
    public Guid Id { get; set; }
    
    [Required]
    public Guid AuthorId { get; set; }
    
    [ForeignKey("AuthorId")]
    public User Author { get; set; }
    
    [Range(0, 5)]
    public double Rating { get; set; }
    
    public string Text { get; set; }
    
    public DateTime AddedAt { get; set; } = DateTime.UtcNow;
}