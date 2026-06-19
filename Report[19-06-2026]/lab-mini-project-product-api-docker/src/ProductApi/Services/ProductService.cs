using Microsoft.EntityFrameworkCore;
using ProductApi.Dtos;
using ProductApi.Models;

namespace ProductApi.Services;

public class ProductService : IProductService
{
    private readonly ProductManagementDbContext _context;

    public ProductService(ProductManagementDbContext context)
    {
        _context = context;
    }

    public async Task<List<ProductDto>> GetAllAsync(
        string? keyword,
        int? categoryId,
        decimal? minPrice,
        decimal? maxPrice,
        int page,
        int pageSize)
    {
        page = page < 1 ? 1 : page;
        pageSize = pageSize < 1 ? 10 : pageSize;

        var query = _context.Products
            .Include(product => product.Category)
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(keyword))
        {
            query = query.Where(product => product.Name.Contains(keyword));
        }

        if (categoryId.HasValue)
        {
            query = query.Where(product => product.CategoryId == categoryId.Value);
        }

        if (minPrice.HasValue)
        {
            query = query.Where(product => product.Price >= minPrice.Value);
        }

        if (maxPrice.HasValue)
        {
            query = query.Where(product => product.Price <= maxPrice.Value);
        }

        return await query
            .OrderBy(product => product.Name)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Select(product => new ProductDto
            {
                Id = product.Id,
                Name = product.Name,
                Price = product.Price,
                StockQuantity = product.StockQuantity,
                CategoryId = product.CategoryId,
                CategoryName = product.Category.Name,
                CreatedAt = product.CreatedAt
            })
            .ToListAsync();
    }

    public async Task<ProductDto?> GetByIdAsync(int id)
    {
        return await _context.Products
            .Include(product => product.Category)
            .Where(product => product.Id == id)
            .Select(product => new ProductDto
            {
                Id = product.Id,
                Name = product.Name,
                Price = product.Price,
                StockQuantity = product.StockQuantity,
                CategoryId = product.CategoryId,
                CategoryName = product.Category.Name,
                CreatedAt = product.CreatedAt
            })
            .FirstOrDefaultAsync();
    }

    public async Task<ProductDto?> CreateAsync(ProductCreateDto dto)
    {
        var categoryExists = await _context.Categories.AnyAsync(category => category.Id == dto.CategoryId);

        if (!categoryExists)
        {
            return null;
        }

        var product = new Product
        {
            Name = dto.Name.Trim(),
            Price = dto.Price,
            StockQuantity = dto.StockQuantity,
            CategoryId = dto.CategoryId,
            CreatedAt = DateTime.UtcNow
        };

        _context.Products.Add(product);
        await _context.SaveChangesAsync();

        return await GetByIdAsync(product.Id);
    }

    public async Task<bool> UpdateAsync(int id, ProductUpdateDto dto)
    {
        var categoryExists = await _context.Categories.AnyAsync(category => category.Id == dto.CategoryId);

        if (!categoryExists)
        {
            return false;
        }

        var product = await _context.Products.FirstOrDefaultAsync(item => item.Id == id);

        if (product is null)
        {
            return false;
        }

        product.Name = dto.Name.Trim();
        product.Price = dto.Price;
        product.StockQuantity = dto.StockQuantity;
        product.CategoryId = dto.CategoryId;

        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var product = await _context.Products.FirstOrDefaultAsync(item => item.Id == id);

        if (product is null)
        {
            return false;
        }

        _context.Products.Remove(product);
        await _context.SaveChangesAsync();
        return true;
    }
}

