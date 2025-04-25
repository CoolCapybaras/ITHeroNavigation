using System.Security.Claims;
using Domain.DTO;
using Domain.Interfaces;
using Domain.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Location = Domain.Models.Location;

namespace ITHeroNavigation.Controllers;

[Route("api/place")]
[ApiController]
public class PlaceController: ControllerBase
{
    private readonly IPlaceService _placeService;

    public PlaceController(IPlaceService placeService)
    {
        _placeService = placeService;
    }
    
    [HttpPost("add_place")]
    [Authorize]
    public async Task<IActionResult> AddPlace([FromBody] PlaceRequest place)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var result = await _placeService.AddPlaceAsync(place, userId);
        if (result.IsSuccess)
            return Ok(new { result = result.Value });
        return BadRequest(new { error = result.Error });
    }
    
    [HttpGet("get_places")]
    [Authorize]
    public async Task<IActionResult> GetPlacesByLoc(double latitude, double longitude, double distanceKm)
    {
       var location = new Location 
       {
           Latitude = latitude,
           Longitude = longitude
       }; 
       var result =  await _placeService.GetPlacesByLocAsync(location, distanceKm);
       if (result.IsSuccess)
           return Ok(new { result = result.Value });
       return BadRequest(new { error = result.Error });
    }

    [HttpPost("add_review")]
    [Authorize]
    public async Task<IActionResult> AddReviewAsync(ReviewRequest review)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var result = await _placeService.AddReviewAsync(review, userId);
        if (result.IsSuccess)
            return Ok(new { result = result.Value });
        return BadRequest(new { error = result.Error });
    }

    [HttpGet("get_review")]
    [Authorize]
    public async Task<IActionResult> GetReviewByPlaceAsync(Guid placeId, int offset, int count)
    {
        var result = await _placeService.GetReviewByPlaceAsync(placeId, offset, count);
        if (result.IsSuccess)
            return Ok(new { result = result.Value });
        return BadRequest(new { error = result.Error });
    }
    
    [HttpGet("get_favorite_places")]
    [Authorize]
    public async Task<IActionResult> GetFavoritePlacesAsync(Guid userId, int offset, int count)
    {
        var result = await _placeService.GetFavoritePlacesAsync(userId, offset, count);
        if (result.IsSuccess)
            return Ok(new { result = result.Value });
        return BadRequest(new { error = result.Error });
    }

    [HttpPost("add_favorite_place")]
    [Authorize]
    public async Task<IActionResult> AddFavoritePlaceAsync(FavoriteRequest favorite)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var result = await _placeService.AddFavoritePlaceAsync(favorite, userId);
        if (result.IsSuccess)
            return Ok(new { result = result.Value });
        return BadRequest(new { error = result.Error });
    }

    [HttpDelete("delete_favorite_place")]
    [Authorize]
    public async Task<IActionResult> DeleteFavoriteaAsync(Guid favoriteId)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var result = await _placeService.DeleteFavoriteaAsync(favoriteId, userId);
        if (result.IsSuccess)
            return Ok(new { result = result.Value });
        return BadRequest(new { error = result.Error });
    }
}