using System.ComponentModel.DataAnnotations;

namespace Domain.DTO;

public class ReviewRequest
{
    public double Rating { get; set; }
    
    public string Text { get; set; }
    
    public Guid PlaceId { get; set; }

}