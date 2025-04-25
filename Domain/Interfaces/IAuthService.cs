using Domain.Errors;

namespace Domain.Interfaces;

public interface IAuthService
{
    Task<Result<string>> RegisterAsync(string email, string password, string username);
    Task<Result<string>> LoginAsync(string email, string password);
    Task<Result<bool>> LogoutAsync();
}