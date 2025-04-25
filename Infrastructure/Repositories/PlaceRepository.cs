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

    public async Task<Place?> GetPlaceById(Guid placeId)
    {
        return await _context.Places.FirstOrDefaultAsync(u => u.Id == placeId);
    }

    public async Task<List<Place>> GetPlacesByLocationAsync(double minLat, double maxLat, double minLon, double maxLon)
    {
        return await _context.Places.Where(p =>
            p.Location.Latitude >= minLat && p.Location.Latitude <= maxLat &&
            p.Location.Longitude >= minLon && p.Location.Longitude <= maxLon).ToListAsync();
    }

    public async Task AddReviewAsync(Review review)
    {
        await _context.Reviews.AddAsync(review);
        await _context.SaveChangesAsync();
    }

    public async Task<List<Review>> GetReviewAsync(Guid placeId, int offset, int count)
    {
        return await _context.Reviews.Where(u => u.PlaceId == placeId)
            .Skip(offset)
            .Take(count)
            .ToListAsync();
    }
}