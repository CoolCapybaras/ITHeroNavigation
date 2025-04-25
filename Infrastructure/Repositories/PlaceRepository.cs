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

    public async Task<List<Review>> GetReviewAsync(Guid placeId, int offset, int count)
    {
        return await _context.Reviews.Where(u => u.PlaceId == placeId)
            .Skip(offset)
            .Take(count)
            .ToListAsync();
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