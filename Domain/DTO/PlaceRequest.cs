using Domain.Models;

namespace Domain.DTO;

public class PlaceRequest
{
    public string Name { get; set; }

    public Location Location { get; set; }

    public string Description { get; set; }

    public string Address { get; set; }

    public Guid CategoryId { get; set; }
}