using Domain.DTO;
using Domain.Models;

namespace Domain.Interfaces;

public interface IPlaceService
{
    public Task<Result<Place>> AddPlaceAsync(PlaceRequest place, string userId);

    public Task<Result<Place>> GetPlaceByIdAsync(Guid placeId);

    public Task<Result<List<Place>>> GetPlacesByLocationAsync(Location location, double radiusKm);

    public Task<Result<Review>> AddReviewAsync(Guid placeId, ReviewRequest review, string userId);

    public Task<Result<List<Review>>> GetReviewsAsync(Guid placeId, int offset, int count);
}