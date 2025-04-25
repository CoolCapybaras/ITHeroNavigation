using Domain.DTO;
using Domain.Interfaces;
using Domain.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

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
    
    [HttpPost]
    [Authorize]
    public async Task<IActionResult> AddPlace([FromBody] PlaceRequest placeRequest)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
        var result = await _placeService.AddPlaceAsync(placeRequest, userId);
        if (result.IsSuccess)
            return Ok(new { result = result.Value });
        return BadRequest(new { error = result.Error });
    }

    [HttpGet]
    [Authorize]
    public async Task<IActionResult> GetPlaceById(Guid placeId)
    {
        var result = await _placeService.GetPlaceByIdAsync(placeId);
        if (result.IsSuccess)
            return Ok(new { result = result.Value });
        return BadRequest(new { error = result.Error });
    }

    [HttpGet("nearby")]
    [Authorize]
    public async Task<IActionResult> GetPlacesByLocation(double latitude, double longitude, double distanceKm)
    {
       var location = new Location 
       {
           Latitude = latitude,
           Longitude = longitude
       }; 
       var result =  await _placeService.GetPlacesByLocationAsync(location, distanceKm);
       if (result.IsSuccess)
           return Ok(new { result = result.Value });
       return BadRequest(new { error = result.Error });
    }

    [HttpPost("{placeId}/reviews")]
    [Authorize]
    public async Task<IActionResult> AddReviewAsync(Guid placeId, ReviewRequest reviewRequest)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
        var result = await _placeService.AddReviewAsync(placeId, reviewRequest, userId);
        if (result.IsSuccess)
            return Ok(new { result = result.Value });
        return BadRequest(new { error = result.Error });
    }

    [HttpGet("{placeId}/reviews")]
    [Authorize]
    public async Task<IActionResult> GetReviewsAsync(Guid placeId, int offset, int count)
    {
        var result = await _placeService.GetReviewsAsync(placeId, offset, count);
        if (result.IsSuccess)
            return Ok(new { result = result.Value });
        return BadRequest(new { error = result.Error });
    }
}