using Domain.Interfaces;
using Domain.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;

namespace ITHeroNavigation.Controllers;

[Route("api/auth")]
[ApiController]
public class UserController : ControllerBase
{
    private readonly IUserService _userService;

    public UserController(IUserService userService)
    {
        _userService = userService;
    }
    
    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterRequest request)
    {
        var token = await _userService.RegisterAsync(request.Email, request.Password, request.Username);
        if (token == "User already exists")
            return BadRequest("User already exists");

        return Ok(new { Token = token });
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest request)
    {
        var token = await _userService.LoginAsync(request.Email, request.Password);
        if (token == null)
            return Unauthorized("Invalid email or password");

        return Ok(new { Token = token });
    }

    [HttpPost("logout")]
    [Authorize]
    public async Task<IActionResult> Logout()
    {
        var result = await _userService.LogoutAsync();
        return result ? Ok("Logged out successfully") : BadRequest("Logout failed");
    }

    public class RegisterRequest
    {
        public string Username { get; set; }

        public string Email { get; set; }
            
        public string Password { get; set; }
    }
    
    
}