using Domain.DTO;
using Domain.Errors;
using Domain.Interfaces;
using Domain.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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

    public async Task<Result<List<Place>>> GetPlacesAsync(SearchRequest searchRequest)
    {
        var location = searchRequest.Location;

        double minLat = 0;
        double maxLat = 0;
        double minLon = 0;
        double maxLon = 0;

        if (location.Latitude != 0 || location.Longitude != 0 || searchRequest.DistanceKm != 0)
        {
            const double oneDegreeLatKm = 111.0;
            double oneDegreeLonKm = 111.320 * Math.Cos(location.Latitude * Math.PI / 180);

            double latOffset = searchRequest.DistanceKm / oneDegreeLatKm;
            double lonOffset = searchRequest.DistanceKm / oneDegreeLonKm;

            minLat = location.Latitude - latOffset;
            maxLat = location.Latitude + latOffset;
            minLon = location.Longitude - lonOffset;
            maxLon = location.Longitude + lonOffset;
        }

        return Result<List<Place>>
            .Success(await _placeRepository.GetPlacesAsync(minLat, maxLat, minLon, maxLon, searchRequest.Name, searchRequest.CategoryIds));
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

    public async Task<Result<Photo>> AddPhotoAsync(Guid placeId, IFormFile file, string userId)
    {
        var parseUserId = Guid.Parse(userId);

        var place = await _placeRepository.GetPlaceByIdAsync(placeId);

        if (place == null)
        {
            return Result<Photo>.Failure("Такого заведения не существует");
        }

        if (place.AuthorId != parseUserId)
        {
            return Result<Photo>.Failure("Вы не создатель этого заведения");
        }

        if (file == null || file.Length == 0)
            return Result<Photo>.Failure("Файл не выбран");

        var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "photos");

        if (!Directory.Exists(uploadsFolder))
            Directory.CreateDirectory(uploadsFolder);

        var uuid = Guid.NewGuid();
        var uniqueFileName = uuid.ToString() + Path.GetExtension(file.FileName);
        var filePath = Path.Combine(uploadsFolder, uniqueFileName);

        using (var stream = new FileStream(filePath, FileMode.Create))
        {
            await file.CopyToAsync(stream);
        }

        var newPhoto = new Photo
        {
            Id = uuid,
            PlaceId = placeId
        };

        await _placeRepository.AddPhotoAsync(newPhoto);
        return Result<Photo>.Success(newPhoto);
    }

    public async Task<Result<List<Photo>>> GetPhotosAsync(Guid placeId, int offset, int count)
    {
        return Result<List<Photo>>.Success(await _placeRepository.GetPhotosAsync(placeId, offset, count));
    }
}