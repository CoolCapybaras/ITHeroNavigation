using Domain.DTO;
using Domain.Errors;
using Domain.Models;
using Microsoft.AspNetCore.Http;

namespace Domain.Interfaces;

public interface IPlaceService
{
    public Task<Result<Place>> AddPlaceAsync(PlaceRequest placeRequest, string userId);

    public Task<Result<string>> DeletePlaceAsync(Guid placeId, string userId);

    public Task<Result<Place>> GetPlaceByIdAsync(Guid placeId);

    public Task<Result<List<Place>>> GetPlacesAsync(SearchRequest searchRequest);

    public Task<Result<Review>> AddReviewAsync(Guid placeId, ReviewRequest reviewRequest, string userId);

    public Task<Result<List<Review>>> GetReviewsAsync(Guid placeId, int offset, int count);

    public Task<Result<string>> LikeReviewAsync(Guid reviewId, string userId);

    public Task<Result<string>> UnlikeReviewAsync(Guid reviewId, string userId);

    public Task<Result<Photo>> AddPhotoAsync(Guid placeId, IFormFile file, string userId);

    public Task<Result<List<Photo>>> GetPhotosAsync(Guid placeId, int offset, int count);
}