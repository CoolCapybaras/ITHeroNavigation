using Domain.DTO;
using Domain.Models;
using Location = Domain.Models.Location;

namespace Domain.Interfaces;

public interface IPlaceService
{
    public Task<Result<Place>> AddPlaceAsync(PlaceRequest place, string userId);
    
    public Task<Result<List<Place>>> GetPlacesByLocAsync(Location location, double radiusKm);

    public Task<Result<Review>> AddReviewAsync(ReviewRequest review, string userId);

    public Task<Result<List<Review>>> GetReviewByPlaceAsync(Guid placeId, int offset, int count);

    public Task<Result<List<Favorite>>> GetFavoritePlacesAsync(Guid userId, int offset, int count);

    public Task<Result<string>> AddFavoritePlaceAsync(FavoriteRequest favorite, string userId);

    public Task<Result<string>> DeleteFavoriteaAsync(Guid favoriteId, string userId);
}