using System.Security.Claims;
using Domain.DTO;
using Domain.Interfaces;
using Domain.Models;
using Microsoft.Extensions.Configuration;
using Location = Domain.Models.Location;

namespace Services;

public class PlaceService: IPlaceService
{
    private readonly IPlaceRepository _placeRepository;
    private readonly IConfiguration _configuration;

    public PlaceService(IPlaceRepository placeRepository, IConfiguration configuration)
    {
        _placeRepository = placeRepository;
        _configuration = configuration;
    }
    
    
    public async Task<Result<Place>> AddPlaceAsync(PlaceRequest place, string userId)
    {
        var parseUserId = Guid.Parse(userId);

        var newPlace = new Place
        {
            Name = place.Name,
            Location = place.Location,
            Description = place.Description,
            Address = place.Address,
            AuthorId = parseUserId,
            CategoryId = place.CategoryId
        };


        await _placeRepository.AddPlaceAsync(newPlace);
        return Result<Place>.Success(newPlace);
    }

    public async Task<Result<List<Place>>> GetPlacesByLocAsync(Location location, double distanceKm)
    {
        const double oneDegreeLatKm = 111.0;
        double oneDegreeLonKm = 111.320 * Math.Cos(location.Latitude * Math.PI / 180);

        double latOffset = distanceKm / oneDegreeLatKm;
        double lonOffset = distanceKm / oneDegreeLonKm;

        double minLat = location.Latitude - latOffset;
        double maxLat = location.Latitude + latOffset;
        double minLon = location.Longitude - lonOffset;
        double maxLon = location.Longitude + lonOffset;
        
        return Result<List<Place>>
            .Success(await _placeRepository.GetPlacesByLocAsync(minLat, maxLat, minLon, maxLon));
    }

    public async Task<Result<Review>> AddReviewAsync(ReviewRequest review, string userId)
    {
        var newReview = new Review
        {
            AuthorId = Guid.Parse(userId),
            Rating = Math.Clamp(review.Rating, 0, 5),
            Text = review.Text,
            PlaceId = review.PlaceId,
            AddedAt = DateTime.UtcNow
        };

        return Result<Review>.Success(await _placeRepository.AddReviewAsync(newReview));
        
    }

    public async Task<Result<List<Review>>> GetReviewByPlaceAsync(Guid placeId, int offset, int count)
    {
        return Result<List<Review>>.Success(await _placeRepository.GetReviewByPlaceAsync(placeId, offset, count));
    }

    public async Task<Result<List<Favorite>>> GetFavoritePlacesAsync(Guid userId, int offset, int count)
    {
        return Result<List<Favorite>>.Success(await _placeRepository.GetFavoritePlacesAsync(userId, offset, count));
    }

    public async Task<Result<string>> AddFavoritePlaceAsync(FavoriteRequest favorite, string userId)
    {
        var parseUserId = Guid.Parse(userId);
        
        if (await _placeRepository.GetFavoriteByIdAsync(favorite.PlaceId) != null)
        {
            return Result<string>.Failure("Это заведение уже в избранном");
        }
        
        var newFavorite = new Favorite
        {
            UserId = parseUserId,
            PlaceId = favorite.PlaceId,
            AddedAt = DateTime.UtcNow
        };
        await _placeRepository.AddFavoritePlaceAsync(newFavorite);
        return Result<string>.Success("Ok");
    }

    public async Task<Result<string>> DeleteFavoriteaAsync(Guid favoriteId, string userId)
    {
        var parseUserId = Guid.Parse(userId);

        var res = await _placeRepository.GetFavoriteByIdAsync(favoriteId);
        
        if (res == null)
        {
            return Result<string>.Failure("Этого заведения нет в избранном");
        }

        await _placeRepository.DeleteFavoriteAsync(res);

        return Result<string>.Success("Ok");
    }
}