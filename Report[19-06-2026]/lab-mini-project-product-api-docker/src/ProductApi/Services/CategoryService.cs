using Microsoft.EntityFrameworkCore;
using ProductApi.Dtos;
using ProductApi.Models;

namespace ProductApi.Services;

public class CategoryService : ICategoryService
{
    private readonly ProductManagementDbContext _context;

    public CategoryService(ProductManagementDbContext context)
    {
        _context = context;
    }

    public async Task<List<CategoryDto>> GetAllAsync()
    {
        return await _context.Categories
            .OrderBy(category => category.Name)
            .Select(category => new CategoryDto
            {
                Id = category.Id,
                Name = category.Name,
                Description = category.Description
            })
            .ToListAsync();
    }

    public async Task<CategoryDto?> GetByIdAsync(int id)
    {
        return await _context.Categories
            .Where(category => category.Id == id)
            .Select(category => new CategoryDto
            {
                Id = category.Id,
                Name = category.Name,
                Description = category.Description
            })
            .FirstOrDefaultAsync();
    }

    public async Task<CategoryDto> CreateAsync(CategoryCreateDto dto)
    {
        var category = new Category
        {
            Name = dto.Name.Trim(),
            Description = dto.Description
        };

        _context.Categories.Add(category);
        await _context.SaveChangesAsync();

        return new CategoryDto
        {
            Id = category.Id,
            Name = category.Name,
            Description = category.Description
        };
    }

    public async Task<bool> UpdateAsync(int id, CategoryUpdateDto dto)
    {
        var category = await _context.Categories.FirstOrDefaultAsync(item => item.Id == id);

        if (category is null)
        {
            return false;
        }

        category.Name = dto.Name.Trim();
        category.Description = dto.Description;

        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var category = await _context.Categories.FirstOrDefaultAsync(item => item.Id == id);

        if (category is null)
        {
            return false;
        }

        _context.Categories.Remove(category);
        await _context.SaveChangesAsync();
        return true;
    }
}

