using Domain.DTO;
using Domain.Models;

namespace Domain.Interfaces;

public interface ICategoryService
{
    Task<Result<Category>> AddCategoryAsync(CategoryRequest placeRequest);

    Task<Result<Category>> GetCategoryByIdAsync(Guid categoryId);
}
