using Domain.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ITHeroNavigation.Controllers;

[Route("api/review")]
[ApiController]
public class ReviewController : ControllerBase
{
    private readonly IPlaceService _placeService;

    public ReviewController(IPlaceService placeService)
    {
        _placeService = placeService;
    }

    [HttpPost("{reviewId}/like")]
    [Authorize]
    public async Task<IActionResult> LikeReviewAsync(Guid reviewId)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
        var result = await _placeService.LikeReviewAsync(reviewId, userId);
        if (result.IsSuccess)
            return Ok(new { result = result.Value });
        return BadRequest(new { error = result.Error });
    }

    [HttpDelete("{reviewId}/unlike")]
    [Authorize]
    public async Task<IActionResult> UnlikeReviewAsync(Guid reviewId)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
        var result = await _placeService.UnlikeReviewAsync(reviewId, userId);
        if (result.IsSuccess)
            return Ok(new { result = result.Value });
        return BadRequest(new { error = result.Error });
    }
}
