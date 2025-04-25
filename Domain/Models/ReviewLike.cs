using System.ComponentModel.DataAnnotations;

namespace Domain.Models;

public class ReviewLike
{
    [Key]
    public Guid UserId { get; set; }
    public User User { get; set; }

    [Key]
    public Guid ReviewId { get; set; }
    public Review Review { get; set; }

    public Guid PlaceId { get; set; }
    public Place Place { get; set; }

    public DateTime LikedAt { get; set; } = DateTime.UtcNow;
}
