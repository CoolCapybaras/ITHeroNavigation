using Domain.Interfaces;
using Domain.Models;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class PlaceRepository: IPlaceRepository
{
    private readonly AppDbContext _context;

    public PlaceRepository(AppDbContext context)
    {
        _context = context;
    }
    
    public async Task AddPlaceAsync(Place place)
    {
        await _context.Places.AddAsync(place);
        await _context.SaveChangesAsync();
    }

    public async Task DeletePlaceAsync(Place place)
    {
        _context.Places.Remove(place);
        await _context.SaveChangesAsync();
    }

    public async Task<Place?> GetPlaceByIdAsync(Guid placeId)
    {
        return await _context.Places.FirstOrDefaultAsync(u => u.Id == placeId);
    }

    public async Task<List<Place>> GetPlacesAsync(double minLat, double maxLat, double minLon, double maxLon, string name, List<Guid> categoryIds)
    {
        var query = _context.Places.AsQueryable();

        // Фильтр по локации
        if (minLat != 0 || maxLat != 0 || minLon != 0 || maxLon != 0)
        {
            query = query.Where(p =>
                p.Location.Latitude >= minLat && p.Location.Latitude <= maxLat &&
                p.Location.Longitude >= minLon && p.Location.Longitude <= maxLon);
        }

        // Фильтр по имени
        if (!string.IsNullOrWhiteSpace(name))
        {
            query = query.Where(p => EF.Functions.ILike(p.Name, $"{name}%"));
        }

        // Фильтр по категориям
        if (categoryIds != null && categoryIds.Count != 0)
        {
            query = query.Where(p => categoryIds.Contains(p.CategoryId));
        }

        return await query.ToListAsync();
    }

    public async Task AddReviewAsync(Review review)
    {
        await _context.Reviews.AddAsync(review);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateRatingAsync(Place place)
    {
        place.Rating = place.Reviews.Average(r => r.Rating);
        await _context.SaveChangesAsync();
    }

    public async Task<Review?> GetReviewByIdAsync(Guid reviewId)
    {
        return await _context.Reviews.FirstOrDefaultAsync(u => u.Id == reviewId);
    }

    public async Task AddReviewLikeAsync(ReviewLike reviewLike)
    {
        await _context.ReviewLikes.AddAsync(reviewLike);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateLikesAsync(Review review)
    {
        review.Likes = await _context.ReviewLikes.CountAsync(u => u.ReviewId == review.Id);
        await _context.SaveChangesAsync();
    }

    public async Task<ReviewLike?> GetReviewLikeAsync(Guid userId, Guid reviewId)
    {
        return await _context.ReviewLikes.FirstOrDefaultAsync(u => u.UserId == userId && u.ReviewId == reviewId);
    }

    public async Task DeleteReviewLikeAsync(ReviewLike reviewLike)
    {
        _context.ReviewLikes.Remove(reviewLike);
        await _context.SaveChangesAsync();
    }

    public async Task<List<Review>> GetReviewsAsync(Guid placeId, int offset, int count)
    {
        var reviews = await _context.Reviews.Where(u => u.PlaceId == placeId)
            .Skip(offset)
            .Take(count)
            .ToListAsync();

        var likedReviewIds = await _context.ReviewLikes.Where(u => u.PlaceId == placeId).Select(u => u.ReviewId).ToListAsync();

        foreach (var review in reviews)
        {
            review.IsLiked = likedReviewIds.Contains(review.Id);
        }

        return reviews;
    }

    public async Task AddPhotoAsync(Photo photo)
    {
        await _context.Photos.AddAsync(photo);
        await _context.SaveChangesAsync();
    }

    public async Task<List<Photo>> GetPhotosAsync(Guid placeId, int offset, int count)
    {
        return await _context.Photos.Where(u => u.PlaceId == placeId).Skip(offset).Take(count).ToListAsync();
    }
}