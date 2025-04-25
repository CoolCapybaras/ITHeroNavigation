using System.ComponentModel.DataAnnotations;

namespace Domain.DTO;

public class ReviewRequest
{
    public double Rating { get; set; }
    
    public string Text { get; set; }
}