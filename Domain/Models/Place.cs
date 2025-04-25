using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;

namespace Domain.Models;

public class Place
{
    [Key]
    public Guid Id { get; set; }
    
    [Required]
    public string Name { get; set; }
    
    public Location Location { get; set; }
    
    [Required]
    public string Description { get; set; }
    
    [Required]
    public string Address { get; set; }
    
    [Range(0, 5)]
    public double Rating { get; set; }

    [JsonIgnore]
    public ICollection<Review> Reviews { get; set; }
    
    [Required]
    public Guid AuthorId { get; set; }
    
    [ForeignKey("AuthorId")]
    [JsonIgnore]
    public User Author { get; set; }
    
    public Guid CategoryId { get; set; }
    
    [ForeignKey("CategoryId")]
    [JsonIgnore]
    public Category Category { get; set; }
}

[Owned]
public class Location
{
    public double Latitude { get; set; }
    
    public double Longitude { get; set; }
}