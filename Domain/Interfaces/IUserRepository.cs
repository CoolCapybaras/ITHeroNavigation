using Domain.Models;

namespace Domain.Interfaces;

public interface IUserRepository
{
    public Task<List<Place>> GetPlacesAsync(Guid userId, int offset, int count);

    public Task<List<Favorite>> GetFavoritesAsync(Guid userId, int offset, int count);

    public Task AddFavoriteAsync(Favorite favorite);

    public Task<Favorite?> GetFavoriteByIdAsync(Guid placeId);

    public Task DeleteFavoriteAsync(Favorite favorite);
}
