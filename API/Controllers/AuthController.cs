using Domain.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ITHeroNavigation.Controllers;

[Route("api/auth")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly IAuthService _AuthService;

    public AuthController(IAuthService AuthService)
    {
        _AuthService = AuthService;
    }
    
    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterRequest request)
    {
        var result = await _AuthService.RegisterAsync(request.Email, request.Password, request.Username);
        if (result.IsSuccess)
            return Ok(new { result = new { token = result.Value } });
        return BadRequest(new { error = "User already exists" });
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest request)
    {
        var result = await _AuthService.LoginAsync(request.Email, request.Password);
        if (result.IsSuccess)
            return Ok(new { result = new { token = result.Value } });
        return BadRequest(new { error = "Invalid email or password" });
    }

    [HttpPost("logout")]
    [Authorize]
    public async Task<IActionResult> Logout()
    {
        var result = await _AuthService.LogoutAsync();
        if (result.IsSuccess)
            return Ok(new { result = "Logged out successfully" });
        return BadRequest(new { error = "Logout failed" });
    }

    public class RegisterRequest
    {
        public string Username { get; set; }

        public string Email { get; set; }
            
        public string Password { get; set; }
    }

    public class LoginRequest
    {
        public string Email { get; set; }
        
        public string Password { get; set; }
    }
}