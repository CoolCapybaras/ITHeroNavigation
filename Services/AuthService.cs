using Domain.Interfaces;
using Domain.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace Services;

public class AuthService : IAuthService
{
    private readonly IAuthRepository _userRepository;
    private readonly IConfiguration _configuration;

    public AuthService(IAuthRepository userRepository, IConfiguration configuration)
    {
        _userRepository = userRepository;
        _configuration = configuration;
    }
    
    public async Task<Result<string>> RegisterAsync(string email, string password, string username)
    {
        if (await _userRepository.GetByEmailAsync(email) != null)
            return Result<string>.Failure("User already exists");

        string hashedPassword = HashPassword(password);

        var newUser = new User
        {
            Email = email,
            HashPassword = hashedPassword,
            Username = username
        };

        await _userRepository.AddUserAsync(newUser);
        return Result<string>.Success(GenerateJwtToken(newUser));
    }

    public async Task<Result<string>> LoginAsync(string email, string password)
    {
        var user = await _userRepository.GetByEmailAsync(email);
        if (user == null || user.HashPassword != HashPassword(password))
            return null;

        return Result<string>.Success(GenerateJwtToken(user));
    }

    public async Task<Result<bool>> LogoutAsync()
    {
        return Result<bool>.Success(await Task.FromResult(true));
    }
    
    private string GenerateJwtToken(User user)
    {
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        var now = DateTime.UtcNow;
        var claims = new[]
        {
            new Claim(ClaimTypes.Email, user.Email),
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new Claim(JwtRegisteredClaimNames.Iat, EpochTime.GetIntDate(now).ToString(), ClaimValueTypes.Integer64)
        };

        var token = new JwtSecurityToken(
            issuer: _configuration["Jwt:Issuer"],
            audience: _configuration["Jwt:Audience"],
            claims: claims,
            expires: now.AddHours(1),
            signingCredentials: credentials
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
    
    private string HashPassword(string password)
    {
        using var sha256 = SHA256.Create();
        var bytes = Encoding.UTF8.GetBytes(password);
        var hash = sha256.ComputeHash(bytes);
        return Convert.ToBase64String(hash);
    }
}