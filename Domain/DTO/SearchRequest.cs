using Domain.Models;

namespace Domain.DTO;

public class SearchRequest
{
    public Location Location { get; set; }
    public double DistanceKm { get; set; }
    public string Name { get; set; }
    public List<Guid> CategoryIds { get; set; }
}
