using Domain.Models;

namespace Domain.Interfaces;

public interface IPlaceRepository
{
    public Task<Place> AddPlaceAsync(Place place);

    public Task<Place?> GetPlaceById(Guid placeId);

    public Task<List<Place>> GetPlacesByLocAsync(double minLat, double maxLat, double minLon, double maxLon);

    public Task<Review> AddReviewAsync(Review review);

    public Task<List<Review>> GetReviewByPlaceAsync(Guid placeId, int offset, int count);

    public Task<List<Favorite>> GetFavoritePlacesAsync(Guid userId, int offset, int count);

    public Task AddFavoritePlaceAsync(Favorite favorite);

    public Task<Favorite?> GetFavoriteByIdAsync(Guid favoriteId);

    public Task DeleteFavoriteAsync(Favorite favorite);
}