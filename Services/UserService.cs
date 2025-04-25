using Domain.DTO;
using Domain.Interfaces;
using Domain.Models;
using Microsoft.Extensions.Configuration;

namespace Services;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;
    private readonly IConfiguration _configuration;

    public UserService(IUserRepository placeRepository, IConfiguration configuration)
    {
        _userRepository = placeRepository;
        _configuration = configuration;
    }

    public async Task<Result<List<Place>>> GetPlacesAsync(string userId, int offset, int count)
    {
        var parseUserId = Guid.Parse(userId);
        return Result<List<Place>>.Success(await _userRepository.GetPlacesAsync(parseUserId, offset, count));
    }

    public async Task<Result<List<Favorite>>> GetFavoritesAsync(string userId, int offset, int count)
    {
        var parseUserId = Guid.Parse(userId);
        return Result<List<Favorite>>.Success(await _userRepository.GetFavoritesAsync(parseUserId, offset, count));
    }

    public async Task<Result<string>> AddFavoriteAsync(FavoriteRequest favorite, string userId)
    {
        var parseUserId = Guid.Parse(userId);

        if (await _userRepository.GetFavoriteByIdAsync(favorite.PlaceId) != null)
        {
            return Result<string>.Failure("Это заведение уже в избранном");
        }

        var newFavorite = new Favorite
        {
            UserId = parseUserId,
            PlaceId = favorite.PlaceId,
            AddedAt = DateTime.UtcNow
        };
        await _userRepository.AddFavoriteAsync(newFavorite);
        return Result<string>.Success("Ok");
    }

    public async Task<Result<string>> DeleteFavoriteAsync(Guid placeId, string userId)
    {
        var parseUserId = Guid.Parse(userId);

        var res = await _userRepository.GetFavoriteByIdAsync(placeId);

        if (res == null)
        {
            return Result<string>.Failure("Этого заведения нет в избранном");
        }

        await _userRepository.DeleteFavoriteAsync(res);

        return Result<string>.Success("Ok");
    }
}
