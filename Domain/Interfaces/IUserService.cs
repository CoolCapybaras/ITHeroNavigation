namespace Domain.Interfaces;

public interface IUserService
{
    Task<string> RegisterAsync(string email, string password, string username);
    Task<string> LoginAsync(string email, string password);
    Task<bool> LogoutAsync();
}