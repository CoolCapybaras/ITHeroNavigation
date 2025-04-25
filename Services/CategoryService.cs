using Domain.DTO;
using Domain.Errors;
using Domain.Interfaces;
using Domain.Models;
using Microsoft.Extensions.Configuration;

namespace Services;

public class CategoryService : ICategoryService
{
    private readonly ICategoryRepository _categoryRepository;
    private readonly IConfiguration _configuration;

    public CategoryService(ICategoryRepository categoryRepository, IConfiguration configuration)
    {
        _categoryRepository = categoryRepository;
        _configuration = configuration;
    }


    public async Task<Result<Category>> AddCategoryAsync(CategoryRequest categoryRequest)
    {
        var newCategory = new Category
        {
            Name = categoryRequest.Name
        };


        await _categoryRepository.AddCategoryAsync(newCategory);
        return Result<Category>.Success(newCategory);
    }

    public async Task<Result<Category>> GetCategoryByIdAsync(Guid placeId)
    {
        var place = await _categoryRepository.GetCategoryByIdAsync(placeId);

        if (place == null)
        {
            return Result<Category>.Failure("Такого заведения не существует");
        }

        return Result<Category>.Success(place);
    }
}
