using Domain.DTO;
using Domain.Interfaces;
using Domain.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ITHeroNavigation.Controllers;

[Route("api/users")]
[ApiController]
public class UserController : ControllerBase
{
    private readonly IUserService _userService;

    public UserController(IUserService userService)
    {
        _userService = userService;
    }

    [HttpGet("me")]
    [Authorize]
    public async Task<IActionResult> GetInfoAsync()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
        var result = await _userService.GetInfoAsync(userId);
        if (result.IsSuccess)
            return Ok(new { result = result.Value });
        return BadRequest(new { error = result.Error });
    }

    [HttpGet("me/places")]
    [Authorize]
    public async Task<IActionResult> GetPlacesAsync(int offset, int count)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
        var result = await _userService.GetPlacesAsync(userId, offset, count);
        if (result.IsSuccess)
            return Ok(new { result = result.Value });
        return BadRequest(new { error = result.Error });
    }

    [HttpGet("me/favorites")]
    [Authorize]
    public async Task<IActionResult> GetFavoritesAsync(int offset, int count)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
        var result = await _userService.GetFavoritesAsync(userId, offset, count);
        if (result.IsSuccess)
            return Ok(new { result = result.Value });
        return BadRequest(new { error = result.Error });
    }

    [HttpPost("me/favorites")]
    [Authorize]
    public async Task<IActionResult> AddFavoriteAsync(FavoriteRequest favorite)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
        var result = await _userService.AddFavoriteAsync(favorite, userId);
        if (result.IsSuccess)
            return Ok(new { result = result.Value });
        return BadRequest(new { error = result.Error });
    }

    [HttpDelete("me/favorites/{placeId}")]
    [Authorize]
    public async Task<IActionResult> DeleteFavoriteAsync(Guid placeId)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
        var result = await _userService.DeleteFavoriteAsync(placeId, userId);
        if (result.IsSuccess)
            return Ok(new { result = result.Value });
        return BadRequest(new { error = result.Error });
    }
}
