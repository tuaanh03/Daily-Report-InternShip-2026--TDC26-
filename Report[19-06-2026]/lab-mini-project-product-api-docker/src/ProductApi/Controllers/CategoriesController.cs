using Microsoft.AspNetCore.Mvc;
using ProductApi.Dtos;
using ProductApi.Services;

namespace ProductApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CategoriesController : ControllerBase
{
    private readonly ICategoryService _categoryService;

    public CategoriesController(ICategoryService categoryService)
    {
        _categoryService = categoryService;
    }

    [HttpGet]
    public async Task<ActionResult<List<CategoryDto>>> GetAll()
    {
        var categories = await _categoryService.GetAllAsync();
        return Ok(categories);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<CategoryDto>> GetById(int id)
    {
        var category = await _categoryService.GetByIdAsync(id);

        if (category is null)
        {
            return NotFound();
        }

        return Ok(category);
    }

    [HttpPost]
    public async Task<ActionResult<CategoryDto>> Create(CategoryCreateDto dto)
    {
        if (string.IsNullOrWhiteSpace(dto.Name))
        {
            return BadRequest("Category name is required.");
        }

        var category = await _categoryService.CreateAsync(dto);
        return CreatedAtAction(nameof(GetById), new { id = category.Id }, category);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, CategoryUpdateDto dto)
    {
        if (string.IsNullOrWhiteSpace(dto.Name))
        {
            return BadRequest("Category name is required.");
        }

        var updated = await _categoryService.UpdateAsync(id, dto);

        if (!updated)
        {
            return NotFound();
        }

        return NoContent();
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var deleted = await _categoryService.DeleteAsync(id);

        if (!deleted)
        {
            return NotFound();
        }

        return NoContent();
    }
}

