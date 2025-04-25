using Domain.Models;

namespace Domain.Interfaces;

public interface IAuthRepository
{
    Task<User?> GetByEmailAsync(string email);

    Task AddUserAsync(User? user);
    
}