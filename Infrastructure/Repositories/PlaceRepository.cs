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
    
    public async Task<Place> AddPlaceAsync(Place place)
    {
        await _context.Places.AddAsync(place);
        await _context.SaveChangesAsync();

        return place;
    }

    public async Task<Place?> GetPlaceById(Guid placeId)
    {
        return await _context.Places.FirstOrDefaultAsync(u => u.Id == placeId);
    }

    public async Task<List<Place>> GetPlacesByLocAsync(double minLat, double maxLat, double minLon, double maxLon)
    {
        

        return await _context.Places.Where(p =>
            p.Location.Latitude >= minLat && p.Location.Latitude <= maxLat &&
            p.Location.Longitude >= minLon && p.Location.Longitude <= maxLon).ToListAsync();
    }

    public async Task<Review> AddReviewAsync(Review review)
    {
        await _context.Reviews.AddAsync(review);
        await _context.SaveChangesAsync();

        return review;
    }

    public async Task<List<Review>> GetReviewByPlaceAsync(Guid placeId, int offset, int count)
    {
        return await _context.Reviews.Where(u => u.PlaceId == placeId)
            .Skip(offset)
            .Take(count)
            .ToListAsync();
    }

    public async Task<List<Favorite>> GetFavoritePlacesAsync(Guid userId, int offset, int count)
    {
        return await _context.Favorites.Where(u => u.UserId == userId).Skip(offset).Take(count).ToListAsync();
    }

    public async Task AddFavoritePlaceAsync(Favorite favorite)
    {
        await _context.Favorites.AddAsync(favorite);
        await _context.SaveChangesAsync();
    }

    public async Task<Favorite?> GetFavoriteByIdAsync(Guid favoriteId)
    {
        return await _context.Favorites.FirstOrDefaultAsync(u => u.PlaceId == favoriteId);
    }

    public async Task DeleteFavoriteAsync(Favorite favorite)
    {
        _context.Favorites.Remove(favorite);
        await _context.SaveChangesAsync();
    }
}