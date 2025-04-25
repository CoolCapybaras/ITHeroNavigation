using Domain.Models;

namespace Domain.Interfaces;

public interface IPlaceRepository
{
    public Task AddPlaceAsync(Place place);

    public Task<Place?> GetPlaceByIdAsync(Guid placeId);

    public Task<List<Place>> GetPlacesByLocationAsync(double minLat, double maxLat, double minLon, double maxLon);

    public Task AddReviewAsync(Review review);

    public Task<List<Review>> GetReviewAsync(Guid placeId, int offset, int count);
}