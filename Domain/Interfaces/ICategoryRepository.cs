using Domain.Models;

namespace Domain.Interfaces;

public interface ICategoryRepository
{
    Task AddCategoryAsync(Category category);

    Task<Category?> GetCategoryByIdAsync(Guid categoryId);
}
