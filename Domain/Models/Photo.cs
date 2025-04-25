using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Domain.Models;

public class Photo
{
    [Key]
    public Guid Id { get; set; }

    public Guid PlaceId { get; set; }

    [ForeignKey("PlaceId")]
    [JsonIgnore]
    public Place Place { get; set; }
}
