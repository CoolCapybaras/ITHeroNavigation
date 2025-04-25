using Domain.DTO;
using Domain.Interfaces;
using Domain.Models;
using Microsoft.Extensions.Configuration;

namespace Services;

public class PlaceService : IPlaceService
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

    public async Task<Result<Place>> GetPlaceByIdAsync(Guid placeId)
    {
        var place = await _placeRepository.GetPlaceByIdAsync(placeId);

        if (place == null)
        {
            return Result<Place>.Failure("Такого заведения не существует");
        }

        return Result<Place>.Success(place);
    }

    public async Task<Result<List<Place>>> GetPlacesByLocationAsync(Location location, double distanceKm)
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
            .Success(await _placeRepository.GetPlacesByLocationAsync(minLat, maxLat, minLon, maxLon));
    }

    public async Task<Result<Review>> AddReviewAsync(Guid placeId, ReviewRequest review, string userId)
    {
        var newReview = new Review
        {
            AuthorId = Guid.Parse(userId),
            Rating = Math.Clamp(review.Rating, 0, 5),
            Text = review.Text,
            PlaceId = placeId,
            AddedAt = DateTime.UtcNow
        };

        await _placeRepository.AddReviewAsync(newReview);
        return Result<Review>.Success(newReview);
    }

    public async Task<Result<List<Review>>> GetReviewsAsync(Guid placeId, int offset, int count)
    {
        return Result<List<Review>>.Success(await _placeRepository.GetReviewAsync(placeId, offset, count));
    }
}