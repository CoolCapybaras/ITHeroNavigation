using Domain.Models;

namespace Domain.Interfaces;

public interface IPlaceRepository
{
    public Task AddPlaceAsync(Place place);

    public Task DeletePlaceAsync(Place place);

    public Task<Place?> GetPlaceByIdAsync(Guid placeId);

    public Task<List<Place>> GetPlacesAsync(double minLat, double maxLat, double minLon, double maxLon, string name, List<Guid> categoryIds);

    public Task AddReviewAsync(Review review);

    public Task UpdateRatingAsync(Place place);

    public Task<Review?> GetReviewByIdAsync(Guid reviewId);

    public Task AddReviewLikeAsync(ReviewLike reviewLike);

    public Task UpdateLikesAsync(Review review);

    public Task<ReviewLike?> GetReviewLikeAsync(Guid userId, Guid reviewId);

    public Task DeleteReviewLikeAsync(ReviewLike reviewLike);

    public Task<List<Review>> GetReviewsAsync(Guid placeId, int offset, int count);

    public Task AddPhotoAsync(Photo photo);

    public Task<List<Photo>> GetPhotosAsync(Guid placeId, int offset, int count);
}