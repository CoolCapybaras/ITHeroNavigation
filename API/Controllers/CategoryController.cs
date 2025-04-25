using Domain.DTO;
using Domain.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ITHeroNavigation.Controllers
{
    [Route("api/category")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryService _categoryService;

        public CategoryController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> AddCategory([FromBody] CategoryRequest categoryRequest)
        {
            var result = await _categoryService.AddCategoryAsync(categoryRequest);
            if (result.IsSuccess)
                return Ok(new { result = result.Value });
            return BadRequest(new { error = result.Error });
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetCategoryById(Guid categoryId)
        {
            var result = await _categoryService.GetCategoryByIdAsync(categoryId);
            if (result.IsSuccess)
                return Ok(new { result = result.Value });
            return BadRequest(new { error = result.Error });
        }
    }
}
