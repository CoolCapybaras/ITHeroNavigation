using Domain.Interfaces;
using Domain.Models;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class PhotoRepository : IPhotoRepository
{
    private readonly AppDbContext _context;

    public PhotoRepository(AppDbContext context)
    {
        _context = context;
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
