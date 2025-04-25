using Domain.DTO;
using Domain.Interfaces;
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

    [HttpGet("{placeId}")]
    [Authorize]
    public async Task<IActionResult> GetPlaceById(Guid placeId)
    {
        var result = await _placeService.GetPlaceByIdAsync(placeId);
        if (result.IsSuccess)
            return Ok(new { result = result.Value });
        return BadRequest(new { error = result.Error });
    }

    [HttpPost("search")]
    [Authorize]
    public async Task<IActionResult> GetPlaces([FromBody] SearchRequest searchRequest)
    {
       var result =  await _placeService.GetPlacesAsync(searchRequest);
       if (result.IsSuccess)
           return Ok(new { result = result.Value });
       return BadRequest(new { error = result.Error });
    }

    [HttpDelete("{placeId}")]
    [Authorize]
    public async Task<IActionResult> DeletePlace(Guid placeId)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
        var result = await _placeService.DeletePlaceAsync(placeId, userId);
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

    [HttpPost("{placeId}/photos")]
    [RequestSizeLimit(10_000_000)] // Ограничение размера, например 10MB
    [Authorize]
    public async Task<IActionResult> UploadPhotoAsync(Guid placeId, IFormFile file)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
        var result = await _placeService.AddPhotoAsync(placeId, file, userId);
        if (result.IsSuccess)
            return Ok(new { result = result.Value });
        return BadRequest(new { error = result.Error });
    }

    [HttpGet("{placeId}/photos")]
    [Authorize]
    public async Task<IActionResult> GetPhotosAsync(Guid placeId, int offset, int count)
    {
        var result = await _placeService.GetPhotosAsync(placeId, offset, count);
        if (result.IsSuccess)
            return Ok(new { result = result.Value });
        return BadRequest(new { error = result.Error });
    }
}