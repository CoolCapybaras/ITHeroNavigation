using Domain.DTO;
using Domain.Models;

namespace Domain.Interfaces;

public interface IUserService
{
    public Task<Result<List<Place>>> GetPlacesAsync(string userId, int offset, int count);

    public Task<Result<List<Favorite>>> GetFavoritesAsync(string userId, int offset, int count);

    public Task<Result<string>> AddFavoriteAsync(FavoriteRequest favorite, string userId);

    public Task<Result<string>> DeleteFavoriteAsync(Guid placeId, string userId);
}
