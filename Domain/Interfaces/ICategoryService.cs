using Domain.DTO;
using Domain.Errors;
using Domain.Models;

namespace Domain.Interfaces;

public interface ICategoryService
{
    Task<Result<Category>> AddCategoryAsync(CategoryRequest categoryRequest);

    Task<Result<Category>> GetCategoryByIdAsync(Guid categoryId);
}
