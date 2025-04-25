using Domain.Interfaces;
using Domain.Models;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class UserRepository : IUserRepository
{
    private readonly AppDbContext _context;

    public UserRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<Place>> GetPlacesAsync(Guid userId, int offset, int count)
    {
        return await _context.Places.Where(u => u.AuthorId == userId).Skip(offset).Take(count).ToListAsync();
    }

    public async Task<List<Favorite>> GetFavoritesAsync(Guid userId, int offset, int count)
    {
        return await _context.Favorites.Where(u => u.UserId == userId).Skip(offset).Take(count).ToListAsync();
    }

    public async Task AddFavoriteAsync(Favorite favorite)
    {
        await _context.Favorites.AddAsync(favorite);
        await _context.SaveChangesAsync();
    }

    public async Task<Favorite?> GetFavoriteByIdAsync(Guid placeId)
    {
        return await _context.Favorites.FirstOrDefaultAsync(u => u.PlaceId == placeId);
    }

    public async Task DeleteFavoriteAsync(Favorite favorite)
    {
        _context.Favorites.Remove(favorite);
        await _context.SaveChangesAsync();
    }
}
